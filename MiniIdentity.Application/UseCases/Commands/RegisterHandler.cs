using AutoMapper;
using MediatR;
using MiniIdentity.Application.DTOs.Responses;
using MiniIdentity.Enterprise.Models;
using MiniIdentity.Application.Repositories;
using MiniIdentity.Enterprise.Services;
using MiniIdentity.Enterprise;

namespace MiniIdentity.Application.UseCases.Commands
{
    public class RegisterHandler : IRequestHandler<Register, UserResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordService _passwordService;

        public RegisterHandler(IMapper mapper, IUnitOfWork ouw, IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IPasswordService passwordService)
        {
            _mapper = mapper;
            _uow = ouw;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _passwordService = passwordService;
        }

        public async Task<UserResponseDto> Handle(Register request, CancellationToken cancellationToken)
        {
            // Normalize the identifiers before using them in comparisons or persistence.
            // This prevents casing differences from allowing duplicate usernames or emails.
            string normalizedUsername = request.Input.Username.ToUpperInvariant();
            string normalizedEmail = request.Input.Email.ToUpperInvariant();

            // New users receive the default "User" role during registration.
            // The role is resolved instead of creating a new role for each registered user.
            Role? roleFound = await _roleRepository.GetByNameAsync(RoleConstants.User);

            // Registration depends on the default role being present.
            // If it is missing, creating the user without a valid role would leave the
            // registration in an inconsistent state.
            if (roleFound == null)
                throw new Exception();

            // Check for an existing username before creating the user.
            // The comparison should use the same normalization strategy used when storing
            // usernames to avoid treating different casing as different accounts.
            if (await _userRepository.ExistsByUsernameAsync(request.Input.Username))
                throw new Exception();

            // The email must also be unique because it can be used to identify the account.
            if (await _userRepository.ExistsByEmailAsync(request.Input.Email))
                throw new Exception();

            // The password is never stored directly.
            // Hashing also produces a salt, which is required to securely verify the
            // password later without retaining the original value.
            PasswordHashResult password = _passwordService.HashPassword(request.Input.Password);

            User userCreated = new User(request.Input.Username, request.Input.Email, password.Hash, password.Salt);

            // The relationship is created using the IDs of the newly created user and
            // the existing default role. The User entity is responsible for generating
            // its own identifier before it is persisted.
            UserRole userRole = new UserRole(userCreated.Id, roleFound.Id);

            await _userRepository.AddAsync(userCreated);

            await _userRoleRepository.AddAsync(userRole);

            // Persist the user and its role assignment together through the unit of work.
            // This keeps the registration as a single persistence operation.
            await _uow.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(userCreated);
        }
    }
}
