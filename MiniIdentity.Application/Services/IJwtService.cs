using MiniIdentity.Application.DTOs.Responses;
using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Application.Services
{
    public interface IJwtService
    {
        AccessTokenResult GenerateAccessToken(User user, IEnumerable<string> roles);
    }
}
