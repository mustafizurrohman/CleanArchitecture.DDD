namespace CleanArchitecture.DDD.API.Controllers.Fake;

public interface IFakeDataService
{
    IEnumerable<FakeDoctorAddressDTO> GetValidDoctors(int num);

    IEnumerable<FakeDoctorAddressDTO> GetDoctorsWithUpdatedAddress(IEnumerable<FakeDoctorAddressDTO> doctors, int iteration);

    IEnumerable<FakeDoctorAddressDTO> GetFakeDoctorsWithSomeInvalidData(int num);
}