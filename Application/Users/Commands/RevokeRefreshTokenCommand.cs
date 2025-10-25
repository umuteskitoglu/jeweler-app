using Application.Interfaces;
using Application.Services;
using MediatR;

namespace Application.Users.Commands;

public record RevokeRefreshTokenCommand(string Email, string RefreshToken, string? Reason) : IRequest;

public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenService _refreshTokenService;

    public RevokeRefreshTokenCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _refreshTokenService = refreshTokenService;
    }

    public async Task Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        await _refreshTokenService.RevokeRefreshTokenAsync(user, request.RefreshToken, request.Reason);
    }
}

