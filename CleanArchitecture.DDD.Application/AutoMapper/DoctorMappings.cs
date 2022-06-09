using CleanArchitecture.DDD.Domain.ValueObjects;

namespace CleanArchitecture.DDD.Application.AutoMapper;

public class DoctorMappings : Profile
{
    public DoctorMappings()
    {
        CreateMappings();
    }

    private void CreateMappings()
    {
        CreateMap<Doctor, DoctorCityDTO>();
        // ToString is implicit and it follows AutoMapper Convention
        //.ForMember(dc => dc.Name, src => src.MapFrom(doc => doc.Name.ToString()))
        //.ForMember(dc => dc.Address, src => src.MapFrom(doc => doc.Address.ToString()));

        // 'EDCMExternalID' need not be expicity mapped because the names match
        CreateMap<FakeDoctorAddressDTO, DoctorDTO>()
            .ForMember(dest => dest.Name,
                src => src.MapFrom(doc => Name.Create(doc.Firstname, doc.Lastname, true)))
            .ForMember(dest => dest.Address,
                src => src.MapFrom(addr => new AddressDTO()
                {
                    AddressID = addr.EDCMExternalID, 
                    City = addr.City, 
                    Country = addr.Country, 
                    StreetAddress = addr.StreetAddress, 
                    ZipCode = addr.ZipCode
                }));

    }
}