using System.Collections.Immutable;
using CleanArchitecture.DDD.Infrastructure.Persistence.JSONColumn;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public sealed class SeedPatientsWithMasterDataCommandHandler 
    : BaseHandler, IRequestHandler<SeedPatientsWithMasterDataCommand>
{
    private readonly Faker _faker = new();

    public SeedPatientsWithMasterDataCommandHandler(IAppServices appServices) 
        : base(appServices)
    {
    }

    public async Task Handle(SeedPatientsWithMasterDataCommand request, CancellationToken cancellationToken)
    {
        var patientList = (await PatientMasterData.CreateRandomAsync(DbContext, request.Num))
            .Select(masterData => new Patient()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                MasterData = masterData
            })
            .ToImmutableList();

        await DbContext.Patients.AddRangeAsync(patientList, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

