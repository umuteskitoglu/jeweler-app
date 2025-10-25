using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> CreateRefreshTokenAsync(User user, string refreshToken, DateTime expiresAt, string ipAddress, string userAgent)
    {
        var entry = user.AddRefreshToken(refreshToken, expiresAt, ipAddress, userAgent);
        _dbContext.RefreshTokens.Add(entry);
        await _dbContext.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(User user, string refreshToken)
    {
        var token = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == user.Id && rt.Token == refreshToken)
            .FirstOrDefaultAsync();

        if (token == null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
        {
            return false;
        }

        return true;
    }

    public async Task RevokeRefreshTokenAsync(User user, string refreshToken, string? reason = null)
    {
        var token = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == user.Id && rt.Token == refreshToken)
            .FirstOrDefaultAsync();

        if (token == null)
        {
            return;
        }

        token.Revoke(new AuditInfo(DateTime.UtcNow, user.Id), reason);
        await _dbContext.SaveChangesAsync();
    }
}

