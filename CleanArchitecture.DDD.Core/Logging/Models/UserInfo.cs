namespace CleanArchitecture.DDD.Core.Logging.Models;

public class UserInfo
{
    public string Username { get; init; }
    public string UserId { get; init; }
    public Dictionary<string, IEnumerable<string>> UserClaim { get; init; }
}