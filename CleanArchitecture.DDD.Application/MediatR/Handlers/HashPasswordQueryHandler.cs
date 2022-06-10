using System.Security.Cryptography;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class HashPasswordQueryHandler : IRequestHandler<HashPasswordQuery, string>
{
    private int NumberOfRounds { get; }

    public HashPasswordQueryHandler()
    {
        // Can be made configurable
        // Recommendation: Double every 2 years
        // 100K is recommened for now!
        NumberOfRounds = 10000;
    }

    public Task<string> Handle(HashPasswordQuery request, CancellationToken cancellationToken)
    {
        var salt = RandomNumberGenerator.GetBytes(32);
        
        var hashedPassword = Rfc2898DeriveBytes.Pbkdf2(
            request.Password,
            salt,
            NumberOfRounds,
            HashAlgorithmName.SHA256,
            32);

        var computedPassword = Convert.ToBase64String(hashedPassword)
                       + "."
                       + Convert.ToBase64String(salt)
                       + "."
                       + NumberOfRounds;

        return Task.FromResult(computedPassword);

    }
}