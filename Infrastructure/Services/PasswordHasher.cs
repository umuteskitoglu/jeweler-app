using System.Security.Cryptography;
using Application.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int Iterations = 10000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public PasswordHash HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);
        var result = Convert.ToBase64String(salt.Concat(hash).ToArray());
        return new PasswordHash(result);
    }

    public bool VerifyPassword(string password, PasswordHash passwordHash)
    {
        var decoded = Convert.FromBase64String(passwordHash.Value);
        var salt = decoded.Take(SaltSize).ToArray();
        var storedHash = decoded.Skip(SaltSize).ToArray();

        var hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);

        return storedHash.SequenceEqual(hash);
    }
}

