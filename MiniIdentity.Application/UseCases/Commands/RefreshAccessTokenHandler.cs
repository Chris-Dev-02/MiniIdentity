using MediatR;
using MiniIdentity.Application.DTOs.Responses;
using MiniIdentity.Application.Repositories;
using MiniIdentity.Application.Services;
using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.UseCases.Commands
{
    public class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessToken, RefreshTokenResultDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshAccessTokenHandler(
            IUnitOfWork uow,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService)
        {
            _uow = uow;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<RefreshTokenResultDto> Handle(RefreshAccessToken request, CancellationToken cancellationToken)
        {
            // Find the refresh token used by the client.
            RefreshToken? refreshToken =
                await _refreshTokenRepository.GetByTokenAsync(
                    request.RefreshTokenInput.RefreshToken);

            if (refreshToken is null)
                throw new Exception("Refresh token not found");

            // The refresh token must still be active.
            if (!refreshToken.IsActive)
                throw new Exception("Refresh token is no longer active");

            // Find the user associated with the refresh token.
            User? userFound =
                await _userRepository.GetByIdAsync(refreshToken.UserId);

            if (userFound is null)
                throw new Exception("User not found");

            if (!userFound.IsActive)
                throw new Exception("User is not active");

            if (userFound.IsBlocked)
                throw new Exception("User is blocked");

            // Get the user's roles to include them in the new access token.
            var roles =
                await _userRoleRepository.GetAssignedRoleNamesAsync(
                    userFound.Id);

            // Generate a new access token.
            var accessToken =
                _jwtService.GenerateAccessToken(userFound, roles);

            // Generate a new refresh token for the same session.
            var newRefreshToken =
                _refreshTokenService.CreateRefreshToken(
                    userFound,
                    request.RefreshTokenInput.ClientIp);

            // Rotate the refresh token.
            // The old token is no longer usable and points to its replacement.
            refreshToken.Revoke(newRefreshToken.Token);

            // Add the new refresh token to the user's sessions.
            userFound.AddRefreshToken(newRefreshToken);

            await _uow.SaveChangesAsync();

            return new RefreshTokenResultDto
            {
                AccessToken = accessToken.Token,
                RefreshToken = newRefreshToken.Token,
                TokenType = "Bearer",
                AccessTokenExpiration = accessToken.ExpirationDate,
                RefreshTokenExpiration = newRefreshToken.ExpirationDate
            };
        }
    }
}
