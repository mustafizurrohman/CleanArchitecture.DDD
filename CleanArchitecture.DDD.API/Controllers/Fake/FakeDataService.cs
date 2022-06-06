using CleanArchitecture.DDD.Application.DTO;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

public class FakeDataService : IFakeDataService
{
    private Faker Faker { get; }

    public FakeDataService()
    {
        Faker = new Faker("de");
    }

    public IEnumerable<DoctorDTO> GetDoctors(int num)
    {
        var fakeCountries = new List<string>()
        {
            "Germany",
            "Austria",
            "Switzerland"
        };

        var fakeAddresses = Enumerable.Range(0, num)
            .Select(_ => new AddressDTO
            {
                AddressID = Guid.NewGuid(),
                StreetAddress = Faker.Address.StreetAddress(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                Country = Faker.Random.ArrayElement(fakeCountries.ToArray())
            })
            .ToList();

        var fakeNames = Enumerable.Range(0, num)
            .Select(_ => Name.Create(Faker.Name.FirstName(), Faker.Name.LastName()))
            .ToList();

        var doctors = Enumerable.Range(0, num)
            .Select(_ =>
            {
                var randomName = Faker.Random.ArrayElement(fakeNames.ToArray());
                var randomAddress = Faker.Random.ArrayElement(fakeAddresses.ToArray());

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
            var address = new AddressDTO
            {
                StreetAddress = cachedDoctor.Address.StreetAddress + "2",
                ZipCode = cachedDoctor.Address.ZipCode + "m",
                City = cachedDoctor.Address.City + "2",
                Country = "Modified"
            };

            var modifiedDoctor = new DoctorDTO
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

    public IEnumerable<FakeDoctorAddressDTO> GetFakeDoctors(int num)
    {
        num = Guard.Against.NegativeOrZero(num, nameof(num));

        var faker = new Faker<FakeDoctorAddressDTO>("de")
            .StrictMode(true)
            .RuleFor(da => da.EDCMExternalID, _ => Guid.NewGuid())
            .RuleFor(da => da.Firstname, fake => fake.Name.FirstName())
            .RuleFor(da => da.Lastname, fake => fake.Name.LastName())
            .RuleFor(da => da.StreetAddress, fake => fake.Address.StreetAddress())
            .RuleFor(da => da.ZipCode, fake => fake.Address.ZipCode())
            .RuleFor(da => da.City, fake => fake.Address.City())
            .RuleFor(da => da.Country, fake => fake.Address.Country());

        var generatedFakeDoctors = faker.Generate(num).ToArray();

        // Invalidate entry in 50% of the cases!
        // ReSharper disable once PossibleLossOfFraction
        var numberOfNamesToInvalidate = (int)Math.Ceiling((double)(num/2)) < generatedFakeDoctors.Length
            // ReSharper disable once PossibleLossOfFraction
            ? (int)Math.Ceiling((double)(num / 2))
            : generatedFakeDoctors.Length;

        var doctorsWithAddress = new List<FakeDoctorAddressDTO>();

        for (int i = 0; i < numberOfNamesToInvalidate; i++)
        {
            var currentDoc = generatedFakeDoctors[i];

            var updated = new FakeDoctorAddressDTO()
            {
                EDCMExternalID = i % 2 == 0 ? Guid.Empty : currentDoc.EDCMExternalID,
                Firstname = currentDoc.Firstname + $" {i}  * ",
                Lastname = currentDoc.Lastname + $"{DateTime.Now}  {i}  ",
                StreetAddress = currentDoc.StreetAddress,
                ZipCode = currentDoc.ZipCode,
                City = currentDoc.City,
                Country = currentDoc.Country
            };

            doctorsWithAddress.Add(updated);
        }

        var validDoctors = generatedFakeDoctors
            .Where(doc => !doctorsWithAddress.Select(da => da.EDCMExternalID).Contains(doc.EDCMExternalID))
            .ToList();

        doctorsWithAddress.AddRange(validDoctors);

        if (doctorsWithAddress.Count < num)
        {
            var newValidDoctors = faker.Generate(num - doctorsWithAddress.Count);
            doctorsWithAddress.AddRange(newValidDoctors);
        }

        doctorsWithAddress = doctorsWithAddress
            .OrderBy(_ => Guid.NewGuid())
            .Take(num)
            .ToList();

        return doctorsWithAddress;
    }
}