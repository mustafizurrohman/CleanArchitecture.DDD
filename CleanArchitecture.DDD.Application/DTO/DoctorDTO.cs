using EntityAddress = CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Address;

namespace CleanArchitecture.DDD.Application.DTO;

public class DoctorDTO
{
    public Guid EDCMExternalID { get; init; }

    public Name Name { get; init; }

    public AddressDTO Address { get; init; }
    
    public static Doctor ToDoctor(DoctorDTO doctorDTO)
    {
        var addrDTO = doctorDTO.Address;
        var docAddress = EntityAddress.Create(addrDTO.StreetAddress, addrDTO.ZipCode, addrDTO.City, addrDTO.Country);

        var docName = Name.Copy(doctorDTO.Name, false);

        return Doctor.Create(docName, docAddress, doctorDTO.EDCMExternalID);
    }
    
}