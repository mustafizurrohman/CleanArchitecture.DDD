using System.Security.Cryptography;

namespace CleanArchitecture.DDD.Application.Services;

public class PasswordService : IPasswordService
{
    private HashAlgorithmName HashAlgorithmName { get; }
    private int OutputLength { get; }
    private int SaltLength { get; }
    private int NumberOfRounds { get; }

    public PasswordService(IPasswordComplexityRoundService passwordComplexityRoundService)
    {
        // Reference- https://www.youtube.com/watch?v=cMykd0jScSY

        // Can be made configurable
        HashAlgorithmName = HashAlgorithmName.SHA256;
        OutputLength = 128;

        // US National Institute of Standards and Technology recommendation
        SaltLength = 128;

        NumberOfRounds = passwordComplexityRoundService.GetPasswordRounds();
    }

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);

        var hashedPasswordAsBase64 = HashPasswordAsBase64(password, salt);
        var hashedPassword = new HashedPassword(hashedPasswordAsBase64, salt.AsBase64String(), NumberOfRounds);
        
        return hashedPassword.ToString();
    }
    
    public bool VerifyPassword(string password, string hashedPassword)
    {
        var passwordHash = new HashedPassword(hashedPassword);
        var salt = passwordHash.Salt.ByeArrayFromBase64String();
        var hashedPasswordAsBase64 = HashPasswordAsBase64(password, salt, passwordHash.NumberOfRounds);

        return passwordHash.Hash == hashedPasswordAsBase64;
    }


    private string HashPasswordAsBase64(string password, byte[] salt, int? iterations = null)
    {
        iterations ??= NumberOfRounds;

        var pbkdf2 = Rfc2898DeriveBytes.Pbkdf2(password.ToByteArray(), salt, iterations.Value, HashAlgorithmName, OutputLength);
        var base64Hash = pbkdf2.AsBase64String();
        
        return base64Hash;
    }
    
}