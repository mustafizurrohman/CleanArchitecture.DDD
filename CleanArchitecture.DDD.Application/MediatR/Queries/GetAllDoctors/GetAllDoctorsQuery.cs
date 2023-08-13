namespace CleanArchitecture.DDD.Application.MediatR.Queries.GetAllDoctors;

public record GetAllDoctorsQuery(bool IncludeDeleted = false)
    : IRequest<IEnumerable<DoctorCityDTO>>;
