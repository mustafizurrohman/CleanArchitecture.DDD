using CleanArchitecture.DDD.Application.Exceptions;

namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class HashedPassword
{
    public string Hash { get; }
    public string Salt { get; }
    public int NumberOfRounds { get; }
    private string Separator { get; }

    private HashedPassword()
    {
        Separator = ".";
    }

    public HashedPassword(string hash, string salt, int numberOfRounds) 
        : this()
    {
        Hash = hash; 
        Salt = salt;
        NumberOfRounds = numberOfRounds;
    }

    public HashedPassword(string passwordAsHashString) 
        : this()
    {
        var hashParts = passwordAsHashString.Split(Separator);

        if (hashParts.Length != 3)
            throw new InvalidHashedPasswordException();

        Hash = hashParts[0]; 
        Salt = hashParts[1];

        var parseSuccessful = hashParts[2].TryParse<int>(out var parsedNumberOfRounds);

        if (!parseSuccessful)
            throw new InvalidHashedPasswordException();

        NumberOfRounds = parsedNumberOfRounds;
    }

    /// <summary>
    /// Hash.Salt.NumberOfRounds
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        // Local function- Available from C# 7.0 
        string CombinePassword(params string[] parts)
        {
            return parts
                .Aggregate((p1, p2) => p1 + Separator + p2);
        }

        return CombinePassword(Hash, Salt, NumberOfRounds.ToString());
    }

}
