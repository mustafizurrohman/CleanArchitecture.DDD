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
        return (await CreateRandomAsync(context, 1)).First();
    }

    public static async Task<IEnumerable<PatientMasterData>> CreateRandomAsync(DomainDbContext context, int num)
    {
        var patientMasterDataList = await context
            .Doctors
            .AsQueryable()
            .OrderBy(_ => Guid.NewGuid())
            .Select(doc => doc.FullName)
            .Take(num)
            .AsAsyncEnumerable()
            .Select(primaryDoctor => Create(primaryDoctor, RandomDay(), RandomActive))
            .ToListAsync();

        return patientMasterDataList;
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

