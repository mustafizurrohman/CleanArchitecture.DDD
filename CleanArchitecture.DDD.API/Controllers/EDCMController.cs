using System.Net;
using AutoMapper;
using CleanArchitecture.DDD.Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.DDD.API.Controllers;

public class EDCMController : BaseAPIController
{
    private readonly IEDCMSyncService _iedcmSyncService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="autoMapper"></param>
    public EDCMController(DomainDbContext dbContext, IMapper autoMapper, IEDCMSyncService iedcmSyncService) 
        : base(dbContext, autoMapper)
    {
        _iedcmSyncService = iedcmSyncService;
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("syncDoc", Name = "syncDoc")]
    [SwaggerOperation(
        Summary = "Gets doc from a fake external data service",
        Description = "No or default authentication required",
        OperationId = "SyncDoc",
        Tags = new[] { "EDCM" }
    )]
    [ProducesResponseType(typeof(IEnumerable<DoctorCityDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<IActionResult> SyncDoctors()
    {
        try
        {
            var doctors = await _iedcmSyncService.SyncDoctors();
            return Ok(doctors);
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Internal error");
            return StatusCode((int)HttpStatusCode.GatewayTimeout, $"Support code : {HttpContext.Connection.Id}");
        }
    }
}