using AutoMapper.QueryableExtensions;
using CleanArchitecture.DDD.Application.MediatR.Queries;
using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class GetAllDoctorsQueryHandler : IRequestHandler<GetAllDoctorsQuery, IEnumerable<DoctorCityDTO>>
{
    private readonly DomainDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllDoctorsQueryHandler(DomainDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorCityDTO>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _dbContext.Doctors.AsNoTracking()
            .ProjectTo<DoctorCityDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return doctors;
    }
}