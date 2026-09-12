using MediatR;
using MiniIdentity.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniIdentity.Application.UseCases.Commands
{
    public class LogoutHandler : IRequestHandler<Logout, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutHandler(
            IUnitOfWork uow,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _uow = uow;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<bool> Handle(Logout request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (refreshToken is null)
                throw new Exception("Refresh token not found");

            if (!refreshToken.IsActive)
                return true;

            refreshToken.Revoke();

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
