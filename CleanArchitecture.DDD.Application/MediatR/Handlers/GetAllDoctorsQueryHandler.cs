namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class GetAllDoctorsQueryHandler : BaseHandler, IRequestHandler<GetAllDoctorsQuery, IEnumerable<DoctorCityDTO>>
{
    public GetAllDoctorsQueryHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task<IEnumerable<DoctorCityDTO>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {

        // A join will be performed automatically by AutoMapper using Entity Framework
        var doctors = await DbContext.Doctors.AsNoTracking()
            .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return doctors;
    }
}