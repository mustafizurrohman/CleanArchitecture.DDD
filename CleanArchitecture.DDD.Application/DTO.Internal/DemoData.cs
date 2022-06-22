namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class DemoData
{
    public bool Cached { get; init; }
    public DateTime CreatedDateTime { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string FullName => (Firstname + " " + Lastname).RemoveConsecutiveSpaces();

    
}