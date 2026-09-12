using MediatR;
using MiniIdentity.Application.DTOs.Requests;
using MiniIdentity.Application.DTOs.Responses;

namespace MiniIdentity.Application.UseCases
{
    public record Register(RegisterRequestDto Input) : IRequest<UserResponseDto>;

    public record Login(LoginRequestDto LoginInput) : IRequest<LoginResultDto>;

    public record Logout(string RefreshToken) : IRequest<bool>;
}
