using MiniIdentity.Enterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniIdentity.Application.Services
{
    public interface IRefreshTokenService
    {
        RefreshToken CreateRefreshToken(User user, string ipAddress);
    }
}
