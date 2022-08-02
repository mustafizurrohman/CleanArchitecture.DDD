using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Models;

public class DependencyHealthCheck
{
    public string DependencyName { get; }
    public string Status { get; }
    public TimeSpan Duration { get; }
    public Exception? Exception { get; }
    public IReadOnlyDictionary<string, object> Data { get; }

    public DependencyHealthCheck(
        string dependencyName,
        HealthStatus status,
        TimeSpan duration,
        Exception? exception,
        IReadOnlyDictionary<string, object> data)
    {
        DependencyName = dependencyName;
        Status = status.ToString();
        Duration = duration;
        Exception = exception;
        Data = data;
    }
}
