using System.Security.Cryptography;

namespace CleanArchitecture.DDD.Application.Services;

public class PasswordService : IPasswordService
{
    private HashAlgorithmName HashAlgorithmName { get; }
    private int OutputLength { get; }
    private int SaltLength { get; }
    private int NumberOfRounds { get; }
    private string Separator { get; }

    public PasswordService()
    {
        HashAlgorithmName = HashAlgorithmName.SHA256;
        OutputLength = 32;
        SaltLength = 32;
        // Can be made configurable
        // Recommendation: Double every 2 years
        // 100K is recommended for now!
        NumberOfRounds = 1000000;
        Separator = ".";
    }

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);

        var hashedPassword = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            NumberOfRounds,
            HashAlgorithmName,
            OutputLength);
        
        return ComputePassword(
            Convert.ToBase64String(hashedPassword), 
            Convert.ToBase64String(salt), 
            NumberOfRounds.ToString());
    }

    private string ComputePassword(params string[] parts)
    {
        return parts
            .Aggregate((p1, p2) => p1 + Separator + p2);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var origHashedParts = hashedPassword.Split(Separator);
        var origSalt = Convert.FromBase64String(origHashedParts[1]);
        var origIterations = int.Parse(origHashedParts[2]);
        var origHash = origHashedParts[0];

        var pbkdf2 = Rfc2898DeriveBytes.Pbkdf2(password, origSalt, origIterations, HashAlgorithmName, OutputLength);
        
        var base64Hashed = Convert.ToBase64String(pbkdf2);

        return base64Hashed == origHash;

    }


}