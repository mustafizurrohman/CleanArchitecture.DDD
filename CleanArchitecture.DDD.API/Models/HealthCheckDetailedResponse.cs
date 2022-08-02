using System.Collections;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Models;

public class HealthCheckDetailedResponse : HealthCheckResponse
{
    public IEnumerable<DependencyHealthCheck> DependencyHealthChecks { get; }

    public HealthCheckDetailedResponse(HealthReport healthReport)
        : base(healthReport)
    {
        DependencyHealthChecks = healthReport.Entries
            .Select(ent => ent.Value)
            .Select(hre => new DependencyHealthCheck(hre.Status, hre.Duration, hre.Exception, hre.Data));
    }

}