namespace CleanArchitecture.DDD.Application.MediatR.Queries;

public record GetAllDoctorsQuery(bool IncludeDeleted = false)
    : IRequest<IEnumerable<DoctorCityDTO>>;
