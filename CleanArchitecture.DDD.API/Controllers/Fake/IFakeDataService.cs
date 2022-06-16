using CleanArchitecture.DDD.Application.DTO;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

public interface IFakeDataService
{
    IEnumerable<ExternalFakeDoctorAddressDTO> GetValidDoctors(int num);

    IEnumerable<ExternalFakeDoctorAddressDTO> GetDoctorsWithUpdatedAddress(IEnumerable<ExternalFakeDoctorAddressDTO> doctors, int iteration);

    IEnumerable<ExternalFakeDoctorAddressDTO> GetFakeDoctorsWithSomeInvalidData(int num);
}