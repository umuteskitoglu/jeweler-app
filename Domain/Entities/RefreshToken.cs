using Domain.ValueObjects;

namespace Domain.Entities;

public class RefreshToken
{
    private RefreshToken()
    {
    }

    public RefreshToken(Guid userId, string token, DateTime expiresAt, string? ipAddress, string? userAgent, AuditInfo created)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Refresh token cannot be null or whitespace", nameof(token));

        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked => RevokedAt.HasValue;
    public DateTime? RevokedAt { get; private set; }
    public string? RevokedReason { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public AuditInfo Created { get; private set; }
    public AuditInfo Updated { get; private set; }

    public void Revoke(AuditInfo auditInfo, string? reason = null)
    {
        RevokedAt = auditInfo.At;
        RevokedReason = reason;
        Updated = auditInfo;
    }
}

