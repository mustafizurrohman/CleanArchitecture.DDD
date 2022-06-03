namespace CleanArchitecture.DDD.API.Controllers.Fake;

public class FakeDoctorAddressDTO
{
    public Guid EDCMExternalID { get; init; }

    public string Firstname { get; init; }
    public string Lastname { get; init; }

    public string StreetAddress { get; init; }
    public string ZipCode { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
}