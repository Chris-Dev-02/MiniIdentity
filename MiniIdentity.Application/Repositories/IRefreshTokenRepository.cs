using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}
