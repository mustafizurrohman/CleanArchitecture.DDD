using CleanArchitecture.DDD.Application.DTO;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

public interface IFakeDataService
{
    IEnumerable<FakeDoctorAddressDTO> GetDoctors(int num);

    IEnumerable<FakeDoctorAddressDTO> GetDoctorsWithUpdatedAddress(IEnumerable<FakeDoctorAddressDTO> doctors, int iteration);

    IEnumerable<FakeDoctorAddressDTO> GetFakeDoctors(int num);
}