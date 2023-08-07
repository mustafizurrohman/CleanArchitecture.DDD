using System.Collections.Generic;
using System.Collections.Immutable;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;
using Microsoft.Identity.Client;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public sealed class SeedDoctorsWithAddressesCommandHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<SeedDoctorsWithAddressesCommand>
{
    private readonly Faker _faker = new();

    private readonly List<string> _fakeCities = new()
    {
        "Berlin",
        "Bern",
        "Vienna",
        "Hamburg",
        "Köln",
        "Munich",
        "Stuttgart",
        "Zurich",
        "Graz",
        "Bonn",
        "Göttingen",
        "Rome",
        "Venice"
    };

    private readonly List<string> _fakeCountries = new()
    {
        "Deutschland",
        "Osterreich",
        "Schweiz"
    };


    public async Task Handle(SeedDoctorsWithAddressesCommand request, CancellationToken cancellationToken)
    {
        var doctors = Enumerable.Range(0, request.Num)
            .Select(_ => GetDoctor(true))
            .Chunk(10)
            .ToAsyncEnumerable();

        await foreach (var chunkOfDoctors in doctors.WithCancellation(cancellationToken))
        {
            await DbContext.AddRangeAsync(chunkOfDoctors, cancellationToken);
                
            await DbContext.SaveChangesAsync(cancellationToken);
            Log.Information("Saved chunk of doctor ... ");
        }
    }

    private T RandomElement<T>(IEnumerable<T> enumerable) => _faker.Random.ArrayElement(enumerable.ToArray());

    private string RandomCity => RandomElement(_fakeCities.ToArray());
    private string RandomCountry => RandomElement(_fakeCountries.ToArray());

    // TODO: Try with Async variant to experiment with new code
    private Doctor GetDoctor(bool simulateDelay = false)
    {
        var address = Address.Create(_faker.Address.StreetName(), _faker.Address.ZipCode(), RandomCity, RandomCountry);
            
        var name = Name.Create(_faker.Name.FirstName(), _faker.Name.LastName());

        var doctor = Doctor.Create(name, address, SpecializationEnumExtensions.GetRandomSpecialization());

        if (simulateDelay)
        {
            var waitTimeInMs = _faker.Random.Number(300, 900);
            Thread.Sleep(waitTimeInMs);
            Log.Information("Waited randomly for {creationTime} milliseconds...", waitTimeInMs);
        }

        return doctor;
    }
}
