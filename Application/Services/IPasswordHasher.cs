using Domain.ValueObjects;

namespace Application.Services;

public interface IPasswordHasher
{
    PasswordHash HashPassword(string password);
    bool VerifyPassword(string password, PasswordHash passwordHash);
}

