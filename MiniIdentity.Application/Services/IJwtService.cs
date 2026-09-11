using MiniIdentity.Application.DTOs.Responses;
using MiniIdentity.Enterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniIdentity.Application.Services
{
    public interface IJwtService
    {
        AccessTokenResult GenerateAccessToken(User user, IEnumerable<string> roles);
    }
}
