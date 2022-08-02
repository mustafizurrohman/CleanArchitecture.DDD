using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Models;

public class DependencyHealthCheck
{
    public string Status { get; }
    public TimeSpan Duration { get; }
    public Exception? Exception { get; }
    public IReadOnlyDictionary<string, object> Data { get; }

    public DependencyHealthCheck(
        HealthStatus status,
        TimeSpan duration,
        Exception? exception,
        IReadOnlyDictionary<string, object> data)
    {
        Status = status.ToString();
        Duration = duration;
        Exception = exception;
        Data = data;
    }
}
