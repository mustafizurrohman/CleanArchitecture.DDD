using System.Collections.Immutable;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public sealed class SeedDoctorsWithAddressesCommandHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<SeedDoctorsWithAddressesCommand>
{
    #region -- Private Variables -- 

    #pragma warning disable S3604 // Member initializer values should not be redundant
    private readonly Faker _faker = new();

    private readonly List<string> _fakeCities = new()
    {
        "Berlin", "Bern", "Vienna", "Hamburg", "Köln", "Munich", "Stuttgart", 
        "Zurich", "Graz", "Bonn", "Göttingen", "Rome", "Venice", "Hannover"
    };

    private readonly List<string> _fakeCountries = new()
    {
        "Deutschland", "Osterreich", "Schweiz"
    };
    #pragma warning restore S3604 // Member initializer values should not be redundant

    private T RandomElement<T>(IEnumerable<T> enumerable) => _faker.Random.ArrayElement(enumerable.ToArray());

    private string RandomCity => RandomElement(_fakeCities);
    private string RandomCountry => RandomElement(_fakeCountries);

    #endregion

    public async Task Handle(SeedDoctorsWithAddressesCommand request, CancellationToken cancellationToken)
    {
        var doctors = GetDoctorsChunkedAsyncEnumerable(request.Num, request.WithRandomDelay)
            .WithCancellation(cancellationToken);

        var numberOfDoctorsSaved = 0;

        // Consuming the async stream
        await foreach (var chunkOfDoctors in doctors)
        {
            var doctorsToSave = chunkOfDoctors.ToImmutableList();
            Console.WriteLine();
            Console.WriteLine($"Got a chunk of doctors with {doctorsToSave.Count} doctors... ");

            await DbContext.AddRangeAsync(doctorsToSave, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            
            numberOfDoctorsSaved += doctorsToSave.Count;
            var remaining = request.Num - numberOfDoctorsSaved;
            var completePercentage = numberOfDoctorsSaved.GetPercentageOf(request.Num);
            var remainingPercentage = remaining.GetPercentageOf(request.Num);
            Log.Information("Saved chunk with {numberOfDoctors} doctors... Completed {completed}%. Remaining {remaining} ({remainingPercentage}%) doctors "
                , doctorsToSave.Count
                , completePercentage
                , remaining
                , remainingPercentage);
        }

        Log.Information("Seeding complete ...");
    }

    private Doctor GetDoctor(bool simulateDelay)
    {
        var doctor = Doctor.CreateRandom();

        if (simulateDelay)
        {
            var waitTimeInMs = _faker.Random.Number(9, 25);
            Thread.Sleep(waitTimeInMs);
        }

        return doctor;
    }

    private Task<Doctor> GetDoctorAsync(bool simulateDelay)
    {
        return Task.FromResult(GetDoctor(simulateDelay));
    }

    private async IAsyncEnumerable<Doctor> GetDoctorsAsyncEnumerable(int num, bool simulateDelay)
    {
        foreach (var _ in Enumerable.Range(1, num))
        {
            yield return await GetDoctorAsync(simulateDelay);
        }
    }

    private async IAsyncEnumerable<IEnumerable<Doctor>> GetDoctorsChunkedAsyncEnumerable(int num, bool simulateDelay)
    {
        Console.WriteLine("Starting buffering ...");

        var bufferedEnumerableChunk = GetDoctorsAsyncEnumerable(num, simulateDelay)
            .Buffer(TimeSpan.FromMilliseconds(2000), _faker.Random.Number(125, 150));

        await foreach (var chuckOfDoctors in bufferedEnumerableChunk)
        {
            yield return chuckOfDoctors; 
        }
    }
}
