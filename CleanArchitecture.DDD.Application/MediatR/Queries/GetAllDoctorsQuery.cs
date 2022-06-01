namespace CleanArchitecture.DDD.Application.MediatR.Queries;

public record GetAllDoctorsQuery 
    : IRequest<IEnumerable<DoctorCityDTO>>
{
}