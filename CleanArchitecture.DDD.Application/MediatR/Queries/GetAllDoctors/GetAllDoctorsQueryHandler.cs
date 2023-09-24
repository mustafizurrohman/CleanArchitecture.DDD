using CleanArchitecture.DDD.Application.MediatR.Handlers;

namespace CleanArchitecture.DDD.Application.MediatR.Queries.GetAllDoctors;

public sealed class GetAllDoctorsQueryHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<GetAllDoctorsQuery, IEnumerable<DoctorCityDTO>>
{
    public async Task<IEnumerable<DoctorCityDTO>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctorsQuery = DbContext.Doctors.AsNoTracking();

        if (request.IncludeDeleted)
            doctorsQuery = doctorsQuery.IgnoreQueryFilters();

        // A join will be performed implicitly by AutoMapper using Entity Framework
        var doctors = await doctorsQuery
            .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return doctors;
    }
}