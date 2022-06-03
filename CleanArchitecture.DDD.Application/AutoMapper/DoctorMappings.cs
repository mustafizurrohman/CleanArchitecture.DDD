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

        CreateMap<FakeDoctorAddressDTO, ExternalDoctorAddressDTO>()
            .ForMember(doc => doc.ExStreetAddress, src => src.MapFrom(doc => doc.StreetAddress))
            .ForMember(doc => doc.ExZipCode, src => src.MapFrom(doc => doc.ZipCode))
            .ForMember(doc => doc.ExCity, src => src.MapFrom(doc => doc.City))
            .ForMember(doc => doc.ExCountry, src => src.MapFrom(doc => doc.Country));
    }
}