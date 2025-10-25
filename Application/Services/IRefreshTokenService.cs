using Domain.Entities;

namespace Application.Services;

public interface IRefreshTokenService
{
    Task<string> CreateRefreshTokenAsync(User user, string refreshToken, DateTime expiresAt, string ipAddress, string userAgent);
    Task<bool> ValidateRefreshTokenAsync(User user, string refreshToken);
    Task RevokeRefreshTokenAsync(User user, string refreshToken, string? reason = null);
}

