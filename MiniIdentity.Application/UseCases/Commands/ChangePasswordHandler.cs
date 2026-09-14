using MediatR;
using MiniIdentity.Application.Repositories;
using MiniIdentity.Enterprise.Models;
using MiniIdentity.Enterprise.Services;

namespace MiniIdentity.Application.UseCases.Commands
{
    public class ChangePasswordHandler : IRequestHandler<ChangePassword, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public ChangePasswordHandler(
            IUnitOfWork uow,
            IUserRepository userRepository,
            IPasswordService passwordService)
        {
            _uow = uow;
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<bool> Handle(
            ChangePassword request,
            CancellationToken cancellationToken)
        {
            User? userFound =
                await _userRepository.GetByIdAsync(request.ChangePasswordInput.UserId);

            if (userFound is null)
                throw new Exception("User not found");

            if (!userFound.IsActive)
                throw new Exception("User is not active");

            if (userFound.IsBlocked)
                throw new Exception("User is blocked");

            bool currentPasswordIsValid =
                _passwordService.VerifyPassword(
                    request.ChangePasswordInput.CurrentPassword,
                    userFound.PasswordHash,
                    userFound.Salt);

            if (!currentPasswordIsValid)
                throw new Exception("Current password is incorrect");

            if (request.ChangePasswordInput.CurrentPassword ==
                request.ChangePasswordInput.NewPassword)
            {
                throw new Exception(
                    "New password must be different from current password");
            }

            string salt = _passwordService.GenerateSalt();

            string passwordHash =
                _passwordService.HashPassword(
                    request.ChangePasswordInput.NewPassword,
                    salt);

            userFound.ChangePassword(passwordHash, salt);

            // Changing the password invalidates all existing sessions.
            userFound.RevokeAllRefreshTokens();

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
