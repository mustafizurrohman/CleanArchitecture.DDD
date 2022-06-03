using CleanArchitecture.DDD.Application.DTO;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

public interface IFakeDataService
{
    IEnumerable<DoctorDTO> GetDoctors(int num);

    IEnumerable<DoctorDTO> GetDoctorsWithUpdatedAddress(IEnumerable<DoctorDTO> doctors);

    IEnumerable<FakeDoctorAddressDTO> GetFakeDoctors(int num);
}