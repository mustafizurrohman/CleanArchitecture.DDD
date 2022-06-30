namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class GetAllDoctorsQueryHandler : BaseHandler, IRequestHandler<GetAllDoctorsQuery, IEnumerable<DoctorCityDTO>>
{
    public GetAllDoctorsQueryHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task<IEnumerable<DoctorCityDTO>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctorsQuery = DbContext.Doctors.AsNoTracking();

        if (request.IncludeDeleted)
            doctorsQuery = doctorsQuery.IgnoreQueryFilters();

        // A join will be performed automatically by AutoMapper using Entity Framework
        var doctors = await doctorsQuery
            .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return doctors;
    }
}