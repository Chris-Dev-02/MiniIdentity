using HopperSoftware.Exceptions.Exceptions;
using MediatR;
using MiniIdentity.Application.Repositories;
using MiniIdentity.Enterprise.Exceptions;
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
                throw new NotFoundException(Modules.User, nameof(request.ChangePasswordInput.UserId), "There's no user with that id");

            if (!userFound.IsActive)
                throw new ClientException(Errors.InvalidState, Modules.User, "User isn't active", nameof(userFound.IsActive));

            if (userFound.IsBlocked)
                throw new ClientException(Errors.InvalidState, Modules.User, "User is blocked", nameof(userFound.IsBlocked));

            bool currentPasswordIsValid =
                _passwordService.VerifyPassword(
                    request.ChangePasswordInput.CurrentPassword,
                    userFound.PasswordHash,
                    userFound.Salt);

            if (!currentPasswordIsValid)
                throw new ClientException(Errors.InvalidOperation, Modules.User, "Password isn't correct", nameof(userFound.IsActive));

            if (request.ChangePasswordInput.CurrentPassword ==
                request.ChangePasswordInput.NewPassword)
            {
                throw new ClientException(Errors.InvalidOperation, Modules.User, "New password must be different from current password", nameof(request.ChangePasswordInput.NewPassword));
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
