using CleanArchitecture.DDD.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Controllers;

[AllowAnonymous]
public class HealthController : BaseAPIController
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(IAppServices appServices, HealthCheckService healthCheckService) : base(appServices)
    {
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet(Name = "health")]
    [SwaggerOperation(
        Summary = "Health Check Endpoint",
        Description = DefaultDescription,
        OperationId = "Health Check Endpoint",
        Tags = new[] { "Health" }
    )]
    public async Task<ActionResult> GetHealth()
    {
        var report = await this._healthCheckService.CheckHealthAsync();
        var result = new HealthCheckDetailedResponse(report);

        return report.Status == HealthStatus.Healthy 
            ? Ok(result) 
            : StatusCode((int)HttpStatusCode.ServiceUnavailable, result);
    }

}