using CleanArchitecture.DDD.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.DDD.API.Controllers;

[AllowAnonymous]
public class HealthController(IAppServices appServices, HealthCheckService healthCheckService)
    : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "Health";

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
        Tags = [DefaultControllerTag]
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetHealth(CancellationToken cancellationToken)
    {
        var report = await healthCheckService.CheckHealthAsync(cancellationToken);
        var result = new HealthCheckDetailedResponse(report);

        return report.Status == HealthStatus.Healthy 
            ? Ok(result) 
            : StatusCode((int)HttpStatusCode.ServiceUnavailable, result);
    }

}