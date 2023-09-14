// ReSharper disable PossibleMultipleEnumeration
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
            return Enumerable.Empty<ExternalFakeDoctorAddressDTO>();
        
        var modifiedDoctors = doctors
            .Select(doc => doc.UpdateAddress())
            .ToList();

        return modifiedDoctors;
    }

    public IEnumerable<ExternalFakeDoctorAddressDTO> GetFakeDoctorsWithSomeInvalidData(int num)
    {
        num = Guard.Against.NegativeOrZero(num);

        var generatedFakeDoctors = DoctorFaker.Generate(num).ToArray();

        double randomPercentage = (double)(new Faker()).Random.Number(5, 45) / 100;
        var numberOfNamesToInvalidate = (int)Math.Floor(randomPercentage * generatedFakeDoctors.Length);

        var validDoctors = generatedFakeDoctors
            .Shuffle()
            .Take(generatedFakeDoctors.Length - numberOfNamesToInvalidate)
            .ToList();

        var invalidDoctors = generatedFakeDoctors
            .ExceptBy(validDoctors.Select(vd => vd.EDCMExternalID), doc => doc.EDCMExternalID)
            .Shuffle()
            .Take(numberOfNamesToInvalidate)
            .Select(doc => doc.Invalidate())
            .ToList();

        return validDoctors.Concat(invalidDoctors).Shuffle();
    }

}