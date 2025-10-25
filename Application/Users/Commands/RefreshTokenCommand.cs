using Application.Interfaces;
using Application.Services;
using MediatR;

namespace Application.Users.Commands;

public record RefreshTokenCommand(string Email, string RefreshToken, string IpAddress, string? UserAgent) : IRequest<AuthenticationResult>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<AuthenticationResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        var isValid = await _refreshTokenService.ValidateRefreshTokenAsync(user, request.RefreshToken);
        if (!isValid)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpires = DateTime.UtcNow.AddDays(7);

        await _refreshTokenService.CreateRefreshTokenAsync(user, newRefreshToken, refreshTokenExpires, request.IpAddress, request.UserAgent ?? "unknown");
        await _refreshTokenService.RevokeRefreshTokenAsync(user, request.RefreshToken, "Rotated");

        return new AuthenticationResult(newAccessToken, newRefreshToken, refreshTokenExpires, user.Email, user.FirstName, user.LastName, user.Role);
    }
}

