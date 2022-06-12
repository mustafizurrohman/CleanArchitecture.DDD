using System.Security.Cryptography;
using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Application.Services;

public class PasswordService : IPasswordService
{
    private HashAlgorithmName HashAlgorithmName { get; }
    private int OutputLength { get; }
    private int SaltLength { get; }
    private int NumberOfRounds { get; }

    public PasswordService()
    {
        HashAlgorithmName = HashAlgorithmName.SHA256;
        OutputLength = 32;
        SaltLength = 32;
        // Can be made configurable
        // Recommendation: Double every 2 years
        // 100K is recommended for now!
        NumberOfRounds = 1000000;
    }

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);

        var saltRaw = GetRawSalt(salt);

        var hashedPasswordAsBase64 = HashPasswordAsBase64(password, salt);
        var hashedPassword = new HashedPassword(hashedPasswordAsBase64, salt.AsBase64String(), NumberOfRounds);
        
        return hashedPassword.ToString();
    }
    

    public bool VerifyPassword(string password, string hash)
    {
        var hashedPassword = new HashedPassword(hash);

        var salt = Convert.FromBase64String(hashedPassword.Salt);

        var saltRaw = GetRawSalt(salt);

        var hashedPasswordAsBase64 = HashPasswordAsBase64(password, salt, hashedPassword.NumberOfRounds);

        return hashedPassword.Hash == hashedPasswordAsBase64;
    }

    private string HashPasswordAsBase64(string password, byte[] salt, int? iterations = null)
    {
        iterations ??= NumberOfRounds;

        var pbkdf2 = Rfc2898DeriveBytes.Pbkdf2(password.ToByteArray(), salt, iterations.Value, HashAlgorithmName, OutputLength);

        var base64Hash = pbkdf2.AsBase64String();
        
        return base64Hash;
    }

    private string? GetRawSalt(byte[] byteArray)
    {
        return byteArray
            .Select(byteVal => byteVal.ToString())
            .Aggregate((p1, p2) => p1 + " " + p2);
    }


}