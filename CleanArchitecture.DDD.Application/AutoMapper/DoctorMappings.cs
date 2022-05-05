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
    }
}