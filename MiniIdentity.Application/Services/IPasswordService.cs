using MiniIdentity.Application.DTOs.Responses;

namespace MiniIdentity.Enterprise.Services
{
    public interface IPasswordService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        PasswordHashResult HashPassword(string password);   
        bool VerifyPassword(string password, string passwordHash, string salt);
    }
}
