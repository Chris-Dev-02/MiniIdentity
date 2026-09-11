using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole userRole);

        Task<List<Role>> GetRolesAsync(string userId);

        Task<List<string>> GetAssignedRoleNamesAsync(string userId);
    }
}
