using agenda.Common.Interfaces;
using agenda.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace agenda.Common.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly int _saltSize;
    private readonly int _hashSize;
    private readonly int _iterations;

    public PasswordHasherService(IOptions<PasswordHashingOptions> options)
    {
        var config = options.Value;
        _saltSize = config.SaltSize;
        _hashSize = config.HashSize;
        _iterations = config.Iterations;
    }

    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
        var salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: HashSize
        );

        var result = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

        return Convert.ToBase64String(result);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: HashSize
        );

        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[SaltSize + i] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}
