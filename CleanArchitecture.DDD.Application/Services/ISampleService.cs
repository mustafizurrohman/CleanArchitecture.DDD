namespace CleanArchitecture.DDD.Application.Services;

public interface ISampleService
{
    Task<IEnumerable<Doctor>> TestHttpClient();
}