using AutoMapper;
using MediatR;
using MiniIdentity.Application.DTOs.Requests;
using MiniIdentity.Application.DTOs.Responses;
using MiniIdentity.Application.Repositories;
using MiniIdentity.Application.Services;
using MiniIdentity.Enterprise.Configurations;
using MiniIdentity.Enterprise.Models;
using MiniIdentity.Enterprise.Services;

namespace MiniIdentity.Application.UseCases.Commands
{
    public class LoginHandler : IRequestHandler<Login, LoginResultDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginHandler(IMapper mapper, 
                            IUnitOfWork ouw, 
                            IUserRepository userRepository, 
                            IUserRoleRepository userRoleRepository, 
                            IPasswordService passwordService, 
                            IJwtService jwtService,
                            IRefreshTokenService refreshTokenService)
        {
            _mapper = mapper;
            _uow = ouw;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResultDto> Handle(Login request, CancellationToken cancellationToken)
        {
            // Username takes precedence when both username and email are provided.
            // This keeps the lookup behavior deterministic instead of attempting both credentials.
            User? userFound = await FindUserAsync(request.LoginInput);

            if (userFound is null)
                throw new Exception("Invalid user or password");

            if (!userFound.IsActive)
                throw new Exception("User is not active");

            if (userFound.IsBlocked)
                throw new Exception("User is blocked");

            if (!userFound.HasEmailBeenVerified && DefaultIdentityConfiguration.EmailMustBeVerified)
                throw new Exception("You must verified your account");

            // A temporary lockout takes precedence over password validation.
            // This prevents login attempts from being processed while the lockout is still active.
            if (userFound.LockoutEnd.HasValue &&
                    userFound.LockoutEnd > DateTime.UtcNow)
                throw new Exception("User lockout");


            if(!_passwordService.VerifyPassword(request.LoginInput.Password, userFound.PasswordHash, userFound.Salt))
            {
                userFound.IncrementFailedLoginAttempts();

                // Lock the account only after reaching the configured maximum.
                // The failed-attempt state must be persisted before returning the error.
                if (userFound.HasAchievedMaxFailedLoginAttempts())
                {
                    userFound.LockUntil();

                    await _uow.SaveChangesAsync();

                    throw new Exception("User has achieved max filed login attempts");
                }

                // Persist the failed attempt even when the account has not been locked yet.
                await _uow.SaveChangesAsync();

                throw new Exception("Invalid user or password");
            }

            // A successful login clears the previous failed-attempt counter
            // so old failed attempts do not affect future lockout decisions.
            userFound.ResetFailedLoginAttempts();

            userFound.RegisterLogin();

            // Roles are included in the access token so authorization can be performed
            // without querying the user's roles on every authenticated request.
            var roles = await _userRoleRepository.GetAssignedRoleNamesAsync(userFound.Id);

            var accessToken = _jwtService.GenerateAccessToken(userFound, roles);

            // The refresh token is associated with the client IP so the session
            // can retain information about where the token was originally created.
            var refreshToken = _refreshTokenService.CreateRefreshToken(userFound, request.LoginInput.ClientIp);

            // When multiple sessions are disabled, an existing active refresh token
            // must either be revoked or prevent the new login, depending on configuration.
            if (!DefaultIdentityConfiguration.AllowsMultipleSessions && userFound.RefreshTokens.Any(x => x.IsActive))
            {
                if (DefaultIdentityConfiguration.ShouldRevokeLastSession)
                    userFound.RefreshTokens.FirstOrDefault(x => x.IsActive)?.Revoke(refreshToken.Token);
                else
                    throw new Exception("There's already an active session");
            }

            userFound.AddRefreshToken(refreshToken);

            // Save the successful login, failed-attempt reset, and new refresh token
            // in the same unit-of-work operation.
            await _uow.SaveChangesAsync();

            return new LoginResultDto()
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiration = accessToken.ExpirationDate,
                RefreshTokenExpiration = refreshToken.ExpirationDate,
                User = _mapper.Map<UserResponseDto>(userFound),
                TokenType = "Bearer"
            };
        }

        /// <summary>
        /// Finds the user using the provided username or email.
        /// When both are provided, username is checked first.
        /// </summary>
        /// <param name="loginInput">Credentials provided by the client.</param>
        /// <returns>The matching user, or null when no user is found.</returns>
        private async Task<User?> FindUserAsync(LoginRequestDto loginInput)
        {
            // Username is checked first when available.
            // To keep the lookup case-insensitive, it is normalized before querying.
            if (!string.IsNullOrWhiteSpace(loginInput.Username))
            {
                var user = await _userRepository.GetByUsernameAsync(loginInput.Username.ToUpperInvariant());
                if (user != null)
                    return user;
            }
            // Email is used as a fallback when no matching username was found.
            if (!string.IsNullOrWhiteSpace(loginInput.Email))
            {
                return await _userRepository.GetByEmailAsync(loginInput.Email.ToUpperInvariant());
            }
            // No usable identifier was provided, or neither lookup found a user.
            return null;
        }
    }
}
