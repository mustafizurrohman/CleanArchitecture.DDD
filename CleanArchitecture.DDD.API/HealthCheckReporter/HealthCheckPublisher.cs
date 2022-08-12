using CleanArchitecture.DDD.API.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.HealthCheckReporter;

public class HealthCheckPublisher : IHealthCheckPublisher
{
    /// <summary>
    /// Results can be published to a Application Performance Monitoring System
    /// Or Azure Application Insights, DataDog or Seq
    /// </summary>
    /// <param name="report"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        var healthCheckDetailedResponse = new HealthCheckDetailedResponse(report);

        switch (report)
        {
            case { Status: HealthStatus.Healthy }:
                {
                    Log.Verbose("[HEALTHY]");
                    break;
                }
            case { Status: HealthStatus.Unhealthy }:
                {
                    Log.Error("[UNHEALTHY] {healthReport}", healthCheckDetailedResponse.ToFormattedJsonFailSafe());
                    break;
                }
            case { Status: HealthStatus.Degraded }:
                {
                    Log.Warning("[DEGRADED] {healthReport}", healthCheckDetailedResponse.ToFormattedJsonFailSafe());
                    break;
                }
        }

        return Task.CompletedTask;
    }
}