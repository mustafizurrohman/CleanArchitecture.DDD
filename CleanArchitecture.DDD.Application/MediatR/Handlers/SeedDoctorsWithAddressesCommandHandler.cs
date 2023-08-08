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
    #pragma warning restore S3604 // Member initializer values should not be redundant
    
    #endregion

    public async Task Handle(SeedDoctorsWithAddressesCommand request, CancellationToken cancellationToken)
    {
        var doctors = GetDoctorsChunkedAsyncEnumerable(request.Num, request.WithRandomDelay)
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
            Log.Information("Saved chunk with {numberOfDoctors} doctors... Completed {completed}% Remaining {remaining} doctors "
                , doctorsToSave.Count
                , completePercentage
                , remaining);
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
            var waitTimeInMs = DateTime.Now.Microsecond % 3  == 0
                ? _faker.Random.Number(90, 200)
                : _faker.Random.Number(200, 400);
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
        var bufferedDoctorEnumerable = GetDoctorsAsyncEnumerable(num, simulateDelay)
            .Buffer(TimeSpan.FromSeconds(1), 8);

        await foreach (var chunk in bufferedDoctorEnumerable)
        {
            yield return chunk; 
        }

        #region -- Trivial Implementation -- 

        /*
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
        } */

        #endregion

    }
}
