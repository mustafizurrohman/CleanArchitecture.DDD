using CleanArchitecture.DDD.API.Controllers.BaseController;
using CleanArchitecture.DDD.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Controllers;

// TODO: Best practice???
[AllowAnonymous]
public class HealthController : BaseAPIController
{
    private const string Tag = "Health";

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetHealth(CancellationToken cancellationToken)
    {
        var report = await _healthCheckService.CheckHealthAsync(cancellationToken);
        var result = new HealthCheckDetailedResponse(report);

        return report.Status == HealthStatus.Healthy 
            ? Ok(result) 
            : StatusCode((int)HttpStatusCode.ServiceUnavailable, result);
    }

}