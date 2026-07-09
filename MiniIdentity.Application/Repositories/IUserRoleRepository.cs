using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole userRole);
    }
}
