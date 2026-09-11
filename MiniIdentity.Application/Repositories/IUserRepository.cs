using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task<bool> ExistsByUsernameAsync(string normalizedUsername);
        Task<bool> ExistsByEmailAsync(string normalizedEmail);

        Task<User?> GetByUsernameAsync(string normalizedUsername);
        Task<User?> GetByEmailAsync(string normalizedEmail);
    }
}
