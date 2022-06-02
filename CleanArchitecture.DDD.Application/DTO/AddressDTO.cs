namespace CleanArchitecture.DDD.Application.DTO;

public class AddressDTO
{
    public Guid AddressID { get; init; }
    public string StreetAddress { get; init; }
    public string ZipCode { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
}