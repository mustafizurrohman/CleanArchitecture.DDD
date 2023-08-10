using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using System.Threading;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;

public class PatientMasterData
{
    public string PrimaryDoctor { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool Active { get; set; }

    private static PatientMasterData Create(string primaryDoctor, DateTime dateOfBirth, bool active)
    {
        return new PatientMasterData()
        {
            PrimaryDoctor = primaryDoctor,
            DateOfBirth = dateOfBirth,
            Active = active
        };
    }

    public static PatientMasterData CreateRandom(DomainDbContext context)
    {
        return CreateRandom(context, 1)[0];
    }

    public static List<PatientMasterData> CreateRandom(DomainDbContext context, int num)
    {
        var patientMasterDataList = context
            .Doctors
            .OrderBy(_ => Guid.NewGuid())
            .Select(doc => doc.FullName)
            .Take(num)
            .AsEnumerable()
            .Select(primaryDoctor => Create(primaryDoctor, RandomDay(), RandomActive))
            .ToList();

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

