using Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities;

public class User
{
    private User()
    {
    }

    public User(string email, PasswordHash passwordHash, string firstName, string lastName, string role, AuditInfo created)
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Role { get; private set; } = null!;
    public AuditInfo Created { get; private set; } = null!;
    public AuditInfo Updated { get; private set; } = null!;
    public AuditInfo? Deleted { get; private set; }
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    public bool IsDeleted => Deleted?.At.HasValue ?? false;

    public void UpdateNames(string firstName, string lastName, AuditInfo auditInfo)
    {
        FirstName = firstName;
        LastName = lastName;
        Updated = auditInfo;
    }

    public void UpdatePassword(PasswordHash passwordHash, AuditInfo auditInfo)
    {
        PasswordHash = passwordHash;
        Updated = auditInfo;
    }

    public void SetRole(string role, AuditInfo auditInfo)
    {
        Role = role;
        Updated = auditInfo;
    }

    public void Delete(AuditInfo deleted)
    {
        Deleted = deleted;
    }

    public void Restore()
    {
        Deleted = null;
    }

    public RefreshToken AddRefreshToken(string token, DateTime expiresAt, string? ipAddress, string? userAgent)
    {
        var refreshToken = new RefreshToken(Id, token, expiresAt, ipAddress, userAgent, new AuditInfo(DateTime.UtcNow, Id));
        RefreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public void RevokeRefreshToken(string token, AuditInfo auditInfo, string? reason = null)
    {
        var refreshToken = RefreshTokens.FirstOrDefault(rt => rt.Token == token);
        refreshToken?.Revoke(auditInfo, reason);
    }
}

