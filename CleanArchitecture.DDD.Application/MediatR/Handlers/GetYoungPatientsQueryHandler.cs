namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class GetYoungPatientsQueryHandler : BaseHandler,
    IRequestHandler<GetYoungPatientsQuery, IEnumerable<PatientMasterDataDTO>>
{
    public GetYoungPatientsQueryHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task<IEnumerable<PatientMasterDataDTO>> Handle(GetYoungPatientsQuery request, CancellationToken cancellationToken)
    {
        // <Patient, PatientMasterDataDTO>

        var query = DbContext.Patients
            .Where(p => p.MasterData.DateOfBirth > DateTime.Now.AddYears(-1 * request.Years));

        if (request.IncludeSoftDeleted)
            query = query.IgnoreQueryFilters();

        var patients = await query
            .AsNoTracking()
            .ProjectTo<PatientMasterDataDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return patients;
    }
}

