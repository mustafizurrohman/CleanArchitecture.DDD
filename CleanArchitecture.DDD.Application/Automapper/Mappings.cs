using AutoMapper;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
using CleanArchitecture.DDD.Domain.DTOs; 

namespace CleanArchitecture.DDD.Domain.Automapper;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMappings();
    }

    private void CreateMappings()
    {
        CreateMap<Doctor, DoctorCityDTO>()
            .ForMember(dc => dc.Name, src => src.MapFrom(doc => doc.Name.ToString()))
            .ForMember(dc => dc.Address, src => src.MapFrom(doc => doc.Address.ToString()));
    }
}

