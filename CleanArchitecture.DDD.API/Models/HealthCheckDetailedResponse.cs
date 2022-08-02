﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Models;

public class HealthCheckDetailedResponse : HealthCheckResponse
{
    public IEnumerable<DependencyHealthCheck> DependencyHealthChecks { get; }

    public HealthCheckDetailedResponse(HealthReport healthReport)
        : base(healthReport)
    {
        DependencyHealthChecks = healthReport.Entries
            .Select(ent => new DependencyHealthCheck(ent.Key, ent.Value.Status, ent.Value.Duration, ent.Value.Exception, ent.Value.Data));
    }

}