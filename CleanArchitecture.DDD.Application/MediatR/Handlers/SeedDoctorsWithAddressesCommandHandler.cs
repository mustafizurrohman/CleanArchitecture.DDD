using System.Collections.Generic;
using System.Collections.Immutable;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;
using Microsoft.Identity.Client;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public sealed class SeedDoctorsWithAddressesCommandHandler 
    : BaseHandler, IRequestHandler<SeedDoctorsWithAddressesCommand>
{
    private readonly Faker _faker;

    private readonly List<string> _fakeCities;

    private readonly List<string> _fakeCountries;


    public SeedDoctorsWithAddressesCommandHandler(IAppServices appServices)
        : base(appServices)
    {
        _faker = new Faker();

        _fakeCities = new List<string>()
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

        _fakeCountries = new List<string>()
        {
            "Deutschland",
            "Osterreich",
            "Schweiz"
        };
    }

    public async Task Handle(SeedDoctorsWithAddressesCommand request, CancellationToken cancellationToken)
    {
        var doctors = Enumerable.Range(0, request.Num)
            .Select(_ => GetDoctor())
            .Chunk(10)
            .ToAsyncEnumerable();

        await foreach (var chunkOfDoctors in doctors)
        {
            await DbContext.AddRangeAsync(chunkOfDoctors, cancellationToken);
            Log.Information("Saved chunk of doctor ... ");
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private Doctor GetDoctor()
    {
        var waitTimeInMs = _faker.Random.Number(300, 900);
        Thread.Sleep(waitTimeInMs);
        var address = Address.Create(_faker.Address.StreetName(), _faker.Address.ZipCode(),
            _faker.Random.ArrayElement(_fakeCities.ToArray()), _faker.Random.ArrayElement(_fakeCountries.ToArray()));
            
        var name = Name.Create(_faker.Name.FirstName(), _faker.Name.LastName());

        var doctor = Doctor.Create(name, address, SpecializationEnumExtensions.GetRandomSpecialization());

        Log.Information("Created doctor in {creationTime} milliseconds...", waitTimeInMs);
        return doctor;
    }
}
