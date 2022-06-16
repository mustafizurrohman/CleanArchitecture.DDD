namespace CleanArchitecture.DDD.Application.AutoMapper;

public class AddressMappings : Profile
{
    public AddressMappings()
    {
        CreateMappings();
    }
    private void CreateMappings()
    {
        CreateMap<ExternalFakeDoctorAddressDTO, FakeDoctorAddressDTO>();

    }
}