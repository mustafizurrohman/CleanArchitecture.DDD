using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class HashPasswordVerificationQueryHandler : IRequestHandler<HashPasswordVerificationQuery, bool>
{
    public async Task<bool> Handle(HashPasswordVerificationQuery request, CancellationToken cancellationToken)
    {
        var parts = request.HashedPassword.Split(".");

        if (parts.Length != 3)
            throw new InvalidDataException();

        var hashedPassword = parts[0];
        var salt = parts[1];
        var rounds = Int32.Parse(parts[2]);

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            ToByteArray(request.Password),
            ToByteArray(salt),
            rounds,
            HashAlgorithmName.SHA256,
            32);

        var computedHashedPassword = Convert.ToBase64String(computedHash);

        return computedHashedPassword == hashedPassword;
    }

    private byte[] ToByteArray(string input)
    {
        return Encoding.ASCII.GetBytes(input);
    }
}