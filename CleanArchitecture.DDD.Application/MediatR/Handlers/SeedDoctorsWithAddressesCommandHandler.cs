using System.Collections.Generic;
using System.Collections.Immutable;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
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
        var chunkSize = request.Num > 1000 
            ? 1000 
            : (int)Math.Floor((double)request.Num / 10);

        chunkSize = Math.Min(chunkSize, request.Num);

        Log.Information("Computed chunk size is {chunkSize}", chunkSize);

        var doctors = GetDoctorsChunkedAsyncEnumerable(request.Num, chunkSize, request.WithRandomDelay)
            .WithCancellation(cancellationToken);

        var numberOfDoctorsSaved = 0;

        await foreach (var chunkOfDoctors in doctors)
        {
            var doctorsToSave = chunkOfDoctors.ToImmutableList();
            Console.WriteLine();
            Console.WriteLine($"Got a chunk of doctors with {doctorsToSave.Count} doctors... ");

            await DbContext.AddRangeAsync(doctorsToSave, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            
            numberOfDoctorsSaved += doctorsToSave.Count;
            var remaining = request.Num - numberOfDoctorsSaved;
            var completePercentage = Math.Round(numberOfDoctorsSaved / (double) request.Num * 100, 2);
            Log.Information("Saved chunk with {numberOfDoctors} doctors... Completed {completed}% Remaining {remaining} doctors ", chunkSize, completePercentage, remaining);
        }

        Log.Information("Seeding complete ...");

    }

    private T RandomElement<T>(IEnumerable<T> enumerable) => _faker.Random.ArrayElement(enumerable.ToArray());

    private string RandomCity => RandomElement(_fakeCities);
    private string RandomCountry => RandomElement(_fakeCountries);

    private Doctor GetDoctor(bool simulateDelay)
    {
        var address = Address.Create(_faker.Address.StreetName(), _faker.Address.ZipCode(), RandomCity, RandomCountry);
            
        var name = Name.Create(_faker.Name.FirstName(), _faker.Name.LastName());

        var doctor = Doctor.Create(name, address, SpecializationEnumExtensions.GetRandomSpecialization());

        if (simulateDelay)
        {
            var waitTimeInMs = _faker.Random.Number(3, 10);
            Thread.Sleep(waitTimeInMs);
        }

        return doctor;
    }

    private Task<Doctor> GetDoctorAsync(bool simulateDelay)
    {
        return Task.FromResult(GetDoctor(simulateDelay));
    }

    // TODO: Can this be further optimized?
    private async IAsyncEnumerable<IEnumerable<Doctor>> GetDoctorsChunkedAsyncEnumerable(int num, int chunkSize, bool simulateDelay)
    {
        var doctors = new List<Doctor>();
        chunkSize = Math.Min(chunkSize, num);

        foreach (var _ in Enumerable.Range(1, num))
        {
            doctors.Add(await GetDoctorAsync(simulateDelay));

            if (doctors.Count == chunkSize)
            {
                yield return doctors;
                doctors.Clear();
            }
        }
    }
}
