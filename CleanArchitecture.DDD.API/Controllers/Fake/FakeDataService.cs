using CleanArchitecture.DDD.Application.DTO;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

public class FakeDataService : IFakeDataService
{
    public IEnumerable<DoctorDTO> GetDoctors(int num)
    {
        var faker = new Faker("de");

        var fakeCountries = new List<string>()
        {
            "Germany",
            "Austria",
            "Switzerland"
        };

        var fakeAddresses = Enumerable.Range(0, num)
            .Select(_ => new AddressDTO()
            {
                AddressID = Guid.NewGuid(),
                StreetAddress = faker.Address.StreetAddress(),
                ZipCode = faker.Address.ZipCode(),
                City = faker.Address.City(),
                Country = faker.Random.ArrayElement(fakeCountries.ToArray())
            })
            .ToList();

        var fakeNames = Enumerable.Range(0, num)
            .Select(_ => Name.Create(faker.Name.FirstName(), faker.Name.LastName()))
            .ToList();

        var doctors = Enumerable.Range(0, num)
            .Select(_ =>
            {
                var randomName = faker.Random.ArrayElement(fakeNames.ToArray());
                var randomAddress = faker.Random.ArrayElement(fakeAddresses.ToArray());

                fakeAddresses.Remove(randomAddress);
                fakeNames.Remove(randomName);

                return new DoctorDTO
                {
                    Name = randomName,
                    Address = randomAddress,
                    EDCMExternalID = Guid.NewGuid()
                };
            })
            .ToList();

        return doctors;
    }

    public IEnumerable<DoctorDTO> GetDoctorsWithUpdatedAddress(IEnumerable<DoctorDTO> doctors)
    {
        doctors = doctors.ToList();

        if (!doctors.Any())
        {
            return Enumerable.Empty<DoctorDTO>();
        }

        var modifiedDoctors = new List<DoctorDTO>();

        foreach (var cachedDoctor in doctors)
        {
            var address = new AddressDTO()
            {
                StreetAddress = cachedDoctor.Address.StreetAddress + "2",
                ZipCode = cachedDoctor.Address.ZipCode + "m",
                City = cachedDoctor.Address.City + "2",
                Country = "Modified"
            };

            var modifiedDoctor = new DoctorDTO()
            {
                EDCMExternalID = cachedDoctor.EDCMExternalID,
                // Update Address here 
                Address = address,
                Name = Name.Create(cachedDoctor.Name.Firstname, cachedDoctor.Name.Lastname + " LNM")
            };

            modifiedDoctors.Add(modifiedDoctor);

        }

        return modifiedDoctors;
    }
}