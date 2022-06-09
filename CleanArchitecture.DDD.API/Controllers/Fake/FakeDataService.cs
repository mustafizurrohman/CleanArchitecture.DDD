namespace CleanArchitecture.DDD.API.Controllers.Fake;

public class FakeDataService : IFakeDataService
{
    private Faker Faker { get; }
    private Faker<FakeDoctorAddressDTO> DoctorFaker { get; }

    public FakeDataService()
    {
        Faker = new Faker("de");

        DoctorFaker = new Faker<FakeDoctorAddressDTO>("de")
            .StrictMode(true)
            .RuleFor(da => da.EDCMExternalID, _ => Guid.NewGuid())
            .RuleFor(da => da.Firstname, fake => fake.Name.FirstName())
            .RuleFor(da => da.Lastname, fake => fake.Name.LastName())
            .RuleFor(da => da.StreetAddress, fake => fake.Address.StreetAddress())
            .RuleFor(da => da.ZipCode, fake => fake.Address.ZipCode())
            .RuleFor(da => da.City, fake => fake.Address.City())
            .RuleFor(da => da.Country, fake => fake.Address.Country());
    }

    public IEnumerable<FakeDoctorAddressDTO> GetValidDoctors(int num)
    {
        num = Guard.Against.NegativeOrZero(num, nameof(num));

        return DoctorFaker.Generate(num);
    }

    public IEnumerable<FakeDoctorAddressDTO> GetDoctorsWithUpdatedAddress(IEnumerable<FakeDoctorAddressDTO> doctors, int iteration)
    {
        doctors = doctors.ToList();

        if (!doctors.Any())
        {
            return Enumerable.Empty<FakeDoctorAddressDTO>();
        }

        var modifiedDoctors = new List<FakeDoctorAddressDTO>();

        foreach (var cachedDoctor in doctors)
        {
            var modifiedDoctor = new FakeDoctorAddressDTO
            {
                EDCMExternalID = cachedDoctor.EDCMExternalID,
                Firstname = cachedDoctor.Firstname,
                Lastname = cachedDoctor.Lastname,
                StreetAddress = Faker.Address.StreetAddress() + iteration,
                ZipCode = Faker.Address.ZipCode() + iteration,
                City = Faker.Address.City() + iteration,
                Country = cachedDoctor.Country + iteration
            };

            modifiedDoctors.Add(modifiedDoctor);
        }

        return modifiedDoctors;
    }

    public IEnumerable<FakeDoctorAddressDTO> GetFakeDoctorsWithSomeInvalidData(int num)
    {
        num = Guard.Against.NegativeOrZero(num, nameof(num));

        var generatedFakeDoctors = DoctorFaker.Generate(num).ToArray();

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
            var newValidDoctors = DoctorFaker.Generate(num - doctorsWithAddress.Count);
            doctorsWithAddress.AddRange(newValidDoctors);
        }

        doctorsWithAddress = doctorsWithAddress
            .OrderBy(_ => Guid.NewGuid())
            .Take(num)
            .ToList();

        return doctorsWithAddress;
    }
}