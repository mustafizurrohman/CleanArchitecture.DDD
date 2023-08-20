namespace CleanArchitecture.DDD.Application.DTO;

public class PatientMasterDataDTO
{
    public string Fullname { get; init; } 
    public DateTime DateOfBirth { get; init; }

    protected PatientMasterDataDTO()
    {

    }
    
}
