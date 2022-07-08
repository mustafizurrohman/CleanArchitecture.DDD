using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SeedDoctorsWithAddressesCommandHandler 
    : BaseHandler, IRequestHandler<SeedDoctorsWithAddressesCommand>
{
    public SeedDoctorsWithAddressesCommandHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task<Unit> Handle(SeedDoctorsWithAddressesCommand request, CancellationToken cancellationToken)
    {
        var faker = new Faker();

        var fakeCountries = new List<string>()
        {
            "Deutschland",
            "Osterreich",
            "Schweiz"
        };

        var addresses = Enumerable.Range(0, request.Num)
            .Select(_ => Address.Create(faker.Address.StreetName(), faker.Address.ZipCode(), faker.Address.City(), faker.Random.ArrayElement(fakeCountries.ToArray())))
            .ToList();

        var names = Enumerable.Range(0, request.Num)
            .Select(_ => Name.Create(faker.Name.FirstName(), faker.Name.LastName()))
            .ToArray();

        var doctors = Enumerable.Range(0, request.Num)
            .Select(_ =>
            {
                var randomName = faker.Random.ArrayElement(names);
                var randomAddress = faker.Random.ArrayElement(addresses.ToArray());
                var randomSpecialization = SpecializationEnumExtensions.GetRandomSpecialization();

                addresses.Remove(randomAddress);

                return Doctor.Create(randomName, randomAddress, randomSpecialization);
            })
            .ToList();

        await DbContext.AddRangeAsync(doctors, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
