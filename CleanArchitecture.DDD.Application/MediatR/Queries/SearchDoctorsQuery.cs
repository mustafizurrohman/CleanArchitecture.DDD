namespace CleanArchitecture.DDD.Application.MediatR.Queries;

public record SearchDoctorsQuery(string? FirstName, string? LastName, bool And = false)
    : IRequest<IEnumerable<DoctorCityDTO>>;