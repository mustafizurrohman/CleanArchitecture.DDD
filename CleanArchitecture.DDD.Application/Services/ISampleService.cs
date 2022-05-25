using CleanArchitecture.DDD.Application.DTO;

namespace CleanArchitecture.DDD.Application.Services;

public interface ISampleService
{
    Task<IEnumerable<DoctorDTO>> TestHttpClient();
}