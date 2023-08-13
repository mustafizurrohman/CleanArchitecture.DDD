using CleanArchitecture.DDD.Application.MediatR.Handlers;

namespace CleanArchitecture.DDD.Application.MediatR.Queries.GetYoungPatients;

public sealed class GetYoungPatientsQueryHandler : BaseHandler,
    IRequestHandler<GetYoungPatientsQuery, IEnumerable<PatientMasterDataDTO>>
{
    public GetYoungPatientsQueryHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task<IEnumerable<PatientMasterDataDTO>> Handle(GetYoungPatientsQuery request, CancellationToken cancellationToken)
    {
        // Querying a JSON column
        var query = DbContext.Patients
            .AsNoTracking()
            .Where(p => p.MasterData.DateOfBirth > DateTime.Now.AddYears(-1 * request.Years));

        if (request.IncludeSoftDeleted)
            query = query.IgnoreQueryFilters();

        var patients = await query
            .ProjectTo<PatientMasterDataDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return patients;
    }
}