namespace CleanArchitecture.DDD.Application.MediatR.Queries;

public record SearchDoctorsQuery(string? FirstName, string? MiddleName, string? LastName)
    : IRequest<IEnumerable<DoctorCityDTO>>
{

}