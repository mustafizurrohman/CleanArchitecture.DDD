using CleanArchitecture.DDD.API.Models;
using HealthChecks.UI.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.HealthCheckReporter;

public class HealthCheckPublisher : IHealthCheckPublisher
{
    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        Log.Verbose("Running health check ...");

        if (report.Status != HealthStatus.Healthy)
        {
            var healthCheckDetailedResponse = new HealthCheckDetailedResponse(report);
            Log.Warning("Unhealthy {healthStatus}", healthCheckDetailedResponse.ToFormattedJson());
        }
        else
        {
            Log.Verbose("Healthy ...");
        }
        
        return Task.CompletedTask;
    }
}