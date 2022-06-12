using CleanArchitecture.DDD.Application.Exceptions;

namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class HashedPassword
{
    public string Hash { get; init; }
    public string Salt { get; init; }
    public int NumberOfRounds { get; init; }
    private string Separator { get; }

    private HashedPassword()
    {
        Separator = ".";
    }

    public HashedPassword(string hash, string salt, int numberOfRounds) : this()
    {
        Hash = hash; 
        Salt = salt;
        NumberOfRounds = numberOfRounds;
    }

    public HashedPassword(string passwordAsHashString) : this()
    {
        var hashParts = passwordAsHashString.Split(Separator);

        if (hashParts.Length != 3)
            throw new InvalidHashedPasswordException();

        Hash = hashParts[0]; 
        Salt = hashParts[1];

        try
        {
            NumberOfRounds = int.Parse(hashParts[2]);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException or FormatException or OverflowException)
                throw new InvalidHashedPasswordException();
        }

    }

    public override string ToString()
    {
        return ComputePassword(Hash, Salt, NumberOfRounds.ToString());
    }

    private string ComputePassword(params string[] parts)
    {
        return parts
            .Aggregate((p1, p2) => p1 + Separator + p2);
    }
}