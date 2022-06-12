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
        // Reference- https://www.youtube.com/watch?v=cMykd0jScSY

        // Can be made configurable
        HashAlgorithmName = HashAlgorithmName.SHA256;
        OutputLength = 128;

        // US National Institute of Standards and Technology recommendation
        SaltLength = 128;

        // Recommendation: Double every 2 years (Moore's law)
        // Generation time will increase when NumberOfRounds will increase
        // Helps in preventing Brute Force Attacks
        // It is fine if use waits for 2-3 seconds for password verification
        // Adjust this accordingly. 
        // Lower- Faster but less secure. Higher- Slower but more secure
        // Underscores improve readability!
        NumberOfRounds = 100_000;
    }

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);

        var hashedPasswordAsBase64 = HashPasswordAsBase64(password, salt);
        var hashedPassword = new HashedPassword(hashedPasswordAsBase64, salt.AsBase64String(), NumberOfRounds);
        
        return hashedPassword.ToString();
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        var hashedPassword = new HashedPassword(hash);
        var salt = hashedPassword.Salt.ByeArrayFromBase64String();
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
    
}