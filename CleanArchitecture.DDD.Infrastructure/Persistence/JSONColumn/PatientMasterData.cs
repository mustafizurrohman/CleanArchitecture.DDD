using Bogus;
using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;

public class PatientMasterData
{
    public string PrimaryDoctor { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool Active { get; set; }

    private static PatientMasterData Create(string primaryDoctor, DateTime dateOfBirth, bool active)
    {
        return new PatientMasterData
        {
            PrimaryDoctor = primaryDoctor,
            DateOfBirth = dateOfBirth,
            Active = active
        };
    }

    public static async Task<PatientMasterData> CreateRandomAsync(DomainDbContext context)
    {
        return (await CreateRandomAsyncEnumerable(context, 1).ToListAsync())[0];
    }

    public static async IAsyncEnumerable<PatientMasterData> CreateRandomAsyncEnumerable(DomainDbContext context, int num)
    {
        var availableDoctors = await context.Doctors.CountAsync();

        if (availableDoctors == 0)
            throw new InvalidOperationException("Doctors not available");
        
        string[] doctorsNames = await context
            .Doctors
            .AsQueryable()
            .OrderBy(_ => Guid.NewGuid())
            .Select(doc => doc.FullName)
            .Take(num)
            .ToArrayAsync();

        var masterData = Enumerable.Range(1, num)
            .Select(_ => (new Faker()).Random.ArrayElement(doctorsNames))
            .Select(primaryDoctor => Create(primaryDoctor, RandomDay(), RandomActive));

        foreach (var md in masterData)
        {
            yield return md;
        }
    }

    private static bool RandomActive => DateTime.Now.Ticks % 2 == 0;

    private static DateTime RandomDay()
    {
        var now = DateTime.Now;
        var start = now.AddYears(-100);
        var range = (now - start).Days;
        return start.AddDays(new Random().Next(range));
    }
}

