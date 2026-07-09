using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string normalizedName);
    }
}
