using CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;
using System.Drawing;
using System.Security.Cryptography;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SeedPatientsWithMasterDataCommandHandler : BaseHandler, IRequestHandler<SeedPatientsWithMasterDataCommand>
{
    private readonly Faker _faker = new Faker();

    public SeedPatientsWithMasterDataCommandHandler(IAppServices appServices) : base(appServices)
    {
    }

    public async Task Handle(SeedPatientsWithMasterDataCommand request, CancellationToken cancellationToken)
    {
        var patientList = Enumerable.Range(0, request.Num)
            .Select(async _ => new Patient()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                MasterData = await GetPatientMasterData(cancellationToken)
            })
            .Select(p => p.Result)
            .ToList();

        await DbContext.Patients.AddRangeAsync(patientList, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<PatientMasterData> GetPatientMasterData(CancellationToken cancellationToken)
    {
        var primaryDoctor = await DbContext
            .Doctors
            .OrderBy(_ => Guid.NewGuid())
            .Select(doc => doc.FullName)
            .FirstOrDefaultAsync(cancellationToken);


        return new PatientMasterData()
        {
            PrimaryDoctor = primaryDoctor ?? string.Empty,
            DateOfBirth = RandomDay(),
            Active = DateTime.Now.Ticks % 2 == 0
        };

    }

    private DateTime RandomDay()
    {
        DateTime start = new DateTime(1995, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(new Random().Next(range));
    }
}

