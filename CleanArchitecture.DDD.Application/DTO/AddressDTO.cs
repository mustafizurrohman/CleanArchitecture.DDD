namespace CleanArchitecture.DDD.Application.DTO;

public class AddressDTO
{
    public Guid AddressID { get; set; }
    public string StreetAddress { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}