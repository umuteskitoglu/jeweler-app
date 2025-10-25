using Application.Interfaces;
using Application.Services;
using MediatR;

namespace Application.Users.Commands;

public record AuthenticateUserCommand(string Email, string Password, string IpAddress, string? UserAgent) : IRequest<AuthenticationResult>;

public record AuthenticationResult(string AccessToken, string RefreshToken, DateTime ExpiresAt, string Email, string FirstName, string LastName, string Role);

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthenticateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<AuthenticationResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpires = DateTime.UtcNow.AddDays(7);

        await _refreshTokenService.CreateRefreshTokenAsync(user, refreshToken, refreshTokenExpires, request.IpAddress, request.UserAgent ?? "unknown");

        return new AuthenticationResult(accessToken, refreshToken, refreshTokenExpires, user.Email, user.FirstName, user.LastName, user.Role);
    }
}

