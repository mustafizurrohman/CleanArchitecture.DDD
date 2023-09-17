using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.MediatR.Handlers;
using CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;

namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedPatientsWithMasterData;

public sealed class SeedPatientsWithMasterDataCommandHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<SeedPatientsWithMasterDataCommand>
{
    private readonly Faker _faker = new();

    public async Task Handle(SeedPatientsWithMasterDataCommand request, CancellationToken cancellationToken)
    {
        var patientList = await PatientMasterData.CreateRandomAsyncEnumerable(DbContext, request.Num)
            .Select(masterData => new Patient()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                MasterData = masterData
            })
            .ToListAsync(cancellationToken);


        await DbContext.Patients.AddRangeAsync(patientList, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

