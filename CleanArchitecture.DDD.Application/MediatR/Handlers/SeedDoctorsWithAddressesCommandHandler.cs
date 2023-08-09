using System.Collections.Immutable;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public sealed class SeedDoctorsWithAddressesCommandHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<SeedDoctorsWithAddressesCommand>
{

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
            var waitTimeInMs = Faker.Random.Number(9, 25);
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
            .Buffer(TimeSpan.FromMilliseconds(2000), Faker.Random.Number(125, 150));

        await foreach (var chuckOfDoctors in bufferedEnumerableChunk)
        {
            yield return chuckOfDoctors; 
        }
    }
}
