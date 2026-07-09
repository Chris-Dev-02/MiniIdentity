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
            string normalizedUsername = request.Input.Username.ToUpperInvariant();
            string normalizedEmail = request.Input.Email.ToUpperInvariant();

            Role? roleFound = await _roleRepository.GetByNameAsync(RoleConstants.User);

            // TO-DO Use a another exception more specific, like my own exception system
            if (roleFound == null)
                throw new Exception();

            // TO-DO Same previous exception
            if(await _userRepository.ExistsByUsernameAsync(request.Input.Username))
                throw new Exception();

            // TO-DO Same again
            if(await _userRepository.ExistsByEmailAsync(request.Input.Email))
                throw new Exception();

            PasswordHashResult password = _passwordService.HashPassword(request.Input.Password);

            User userCreated = new User(request.Input.Username, request.Input.Email, password.Hash, password.Salt);

            UserRole userRole = new UserRole(userCreated.Id, roleFound.Id);

            await _userRepository.AddAsync(userCreated);

            await _userRoleRepository.AddAsync(userRole);

            await _uow.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(userCreated);
        }
    }
}
