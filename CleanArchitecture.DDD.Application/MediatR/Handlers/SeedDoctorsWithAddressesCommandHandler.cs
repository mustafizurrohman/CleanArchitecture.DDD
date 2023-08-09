using System.Collections.Immutable;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public sealed class SeedDoctorsWithAddressesCommandHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<SeedDoctorsWithAddressesCommand>
{
    private int _numberOfDoctorsSaved;

    public async Task Handle(SeedDoctorsWithAddressesCommand request, CancellationToken cancellationToken)
    {
        var doctors = GetDoctorsChunkedAsyncEnumerable(request.Num, request.WithRandomDelay)
            .WithCancellation(cancellationToken);

        // Consuming the async stream
        await foreach (var chunkOfDoctors in doctors)
        {
            var doctorsToSave = chunkOfDoctors.ToImmutableList();
            Log.Information("Received a chunk of doctors with {numberOfDoctorsReceived} doctors... ", doctorsToSave.Count);

            await DbContext.AddRangeAsync(doctorsToSave, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            
            LogProgress(doctorsToSave.Count, request.Num);
        }

        Log.Information("Seeding complete ...");
    }

    private void LogProgress(int numberOfDoctorsSavedInChunk, int totalNumberOfDoctorsToSave)
    {
        _numberOfDoctorsSaved += numberOfDoctorsSavedInChunk;
        var remaining = totalNumberOfDoctorsToSave - _numberOfDoctorsSaved;
        var completePercentage = _numberOfDoctorsSaved.GetPercentageOf(totalNumberOfDoctorsToSave);
        var remainingPercentage = remaining.GetPercentageOf(totalNumberOfDoctorsToSave);
        Log.Information("Saved chunk with {numberOfDoctors} doctors... Completed {completed}%. Remaining {remaining} ({remainingPercentage}%) doctors "
            , numberOfDoctorsSavedInChunk
            , completePercentage
            , remaining
            , remainingPercentage);
    }

    private Task<Doctor> GetDoctorAsync(bool simulateDelay)
    {
        var doctor = Doctor.CreateRandom();

        if (simulateDelay)
        {
            var waitTimeInMs = Faker.Random.Number(9, 25);
            Thread.Sleep(waitTimeInMs);
        }

        return doctor.AsTask();
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
