namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SeedAddressCommandHandler : BaseHandler, IRequestHandler<SeedAddressCommand>
{
    public SeedAddressCommandHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task<Unit> Handle(SeedAddressCommand request, CancellationToken cancellationToken)
    {
        var faker = new Faker("de");

        var fakeCountries = new List<string>()
        {
            "Deutschland",
            "Osterreich",
            "Schweiz"
        };

        var addresses = Enumerable.Range(0, request.Num)
            .Select(_ => Address.Create(faker.Address.StreetName(), faker.Address.ZipCode(), faker.Address.City(), faker.Random.ArrayElement(fakeCountries.ToArray())))
            .ToList();

        var addressJSON = addresses.ToFormattedJson();

        await DbContext.AddRangeAsync(addresses, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}