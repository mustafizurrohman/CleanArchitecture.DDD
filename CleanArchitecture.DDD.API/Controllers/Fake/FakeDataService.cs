namespace CleanArchitecture.DDD.API.Controllers.Fake;

public class FakeDataService : IFakeDataService
{
    private Faker Faker { get; }
    private Faker<ExternalFakeDoctorAddressDTO> DoctorFaker { get; }

    public FakeDataService()
    {
        Faker = new Faker("de");

        DoctorFaker = new Faker<ExternalFakeDoctorAddressDTO>("de")
            .StrictMode(true)
            .RuleFor(da => da.EDCMExternalID, _ => Guid.NewGuid())
            .RuleFor(da => da.Firstname, fake => fake.Name.FirstName())
            .RuleFor(da => da.Lastname, fake => fake.Name.LastName())
            .RuleFor(da => da.StreetAddress, fake => fake.Address.StreetAddress())
            .RuleFor(da => da.ZipCode, fake => fake.Address.ZipCode())
            .RuleFor(da => da.City, fake => fake.Address.City())
            .RuleFor(da => da.Country, fake => fake.Address.Country());
    }

    public IEnumerable<ExternalFakeDoctorAddressDTO> GetValidDoctors(int num)
    {
        num = Guard.Against.NegativeOrZero(num);

        return DoctorFaker.Generate(num);
    }

    public IEnumerable<ExternalFakeDoctorAddressDTO> GetDoctorsWithUpdatedAddress(IEnumerable<ExternalFakeDoctorAddressDTO> doctors, int iteration)
    {
        doctors = doctors.ToList();

        if (!doctors.Any())
        {
            return Enumerable.Empty<ExternalFakeDoctorAddressDTO>();
        }

        var modifiedDoctors = new List<ExternalFakeDoctorAddressDTO>();

        foreach (var cachedDoctor in doctors)
        {
            var modifiedDoctor = new ExternalFakeDoctorAddressDTO
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

    public IEnumerable<ExternalFakeDoctorAddressDTO> GetFakeDoctorsWithSomeInvalidData(int num)
    {
        num = Guard.Against.NegativeOrZero(num);

        var generatedFakeDoctors = DoctorFaker.Generate(num).ToArray();

        // Invalidate entry in 50% of the cases!
        // ReSharper disable once PossibleLossOfFraction
        var numberOfNamesToInvalidate = (int)Math.Ceiling((double)(num/2)) < generatedFakeDoctors.Length
            // ReSharper disable once PossibleLossOfFraction
            ? (int)Math.Ceiling((double)(num / 2))
            : generatedFakeDoctors.Length;

        var doctorsWithAddress = new List<ExternalFakeDoctorAddressDTO>();

        for (var i = 0; i < numberOfNamesToInvalidate; i++)
        {
            var currentDoc = generatedFakeDoctors[i];

            var updated = new ExternalFakeDoctorAddressDTO()
            {
                EDCMExternalID = i % 2 == 0 ? Guid.Empty : currentDoc.EDCMExternalID,
                Firstname = currentDoc.Firstname + (i % 3 == 0 ? "" : $" {i}  * "),
                Lastname = currentDoc.Lastname + (i % 3 == 0 ? "" : $"{DateTime.Now}  {i}  "),
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