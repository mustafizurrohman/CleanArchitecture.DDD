using JetBrains.Annotations;

namespace CleanArchitecture.DDD.Application.AutoMapper;

[UsedImplicitly]
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