namespace CleanArchitecture.DDD.Application.DTO;

public class ExternalDoctorAddressDTO
{
    public Guid EDCMExternalID { get; init; }

    public string Firstname { get; init; }
    public string Lastname { get; init; }

    public string ExStreetAddress { get; init; }
    public string ExZipCode { get; init; }
    public string ExCity { get; init; }
    public string ExCountry { get; init; }
}