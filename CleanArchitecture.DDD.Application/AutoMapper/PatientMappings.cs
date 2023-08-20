namespace CleanArchitecture.DDD.Application.AutoMapper;

public class PatientMappings : Profile
{
    public PatientMappings()
    {
        CreateMappings();
    }

    private void CreateMappings()
    {
        CreateMap<Patient, PatientMasterDataDTO>()
            .ForMember(dto => dto.DateOfBirth, src => src.MapFrom(p => p.MasterData.DateOfBirth))
            .ForMember(dto => dto.Fullname, src => src.MapFrom(p => p.Fullname));
    }
}

