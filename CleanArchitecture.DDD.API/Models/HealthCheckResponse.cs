﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Models;

public class HealthCheckResponse
{
    public string OverallStatus { get; }
    public TimeSpan TotalChecksDuration { get; }

    public HealthCheckResponse(HealthReport healthReport)
    {
        OverallStatus = healthReport.Status.ToString();
        
        TotalChecksDuration = healthReport.TotalDuration;
    }
}