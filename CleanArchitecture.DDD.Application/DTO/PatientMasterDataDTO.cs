namespace CleanArchitecture.DDD.Application.DTO;

[UsedImplicitly]
public class PatientMasterDataDTO
{
    public string Fullname { get; init; } 
    public DateTime DateOfBirth { get; init; }

    protected PatientMasterDataDTO()
    {

    }
    

}
