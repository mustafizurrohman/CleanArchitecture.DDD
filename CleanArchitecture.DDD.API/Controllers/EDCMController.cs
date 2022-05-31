using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.DDD.API.Controllers;

public class EDCMController : BaseAPIController
{
    private readonly IEDCMSyncService _iedcmSyncService;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="autoMapper"></param>
    /// <param name="iedcmSyncService"></param>
    public EDCMController(DomainDbContext dbContext, IMapper autoMapper, IEDCMSyncService iedcmSyncService, IMediator mediator) 
        : base(dbContext, autoMapper)
    {
        _iedcmSyncService = iedcmSyncService;
        _mediator = mediator;
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("syncDoc", Name = "syncDoc")]
    [SwaggerOperation(
        Summary = "Gets doc from a fake external data service",
        Description = "No or default authentication required",
        OperationId = "SyncDoc",
        Tags = new[] { "EDCM" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<IActionResult> SyncDoctors()
    {
        try
        {
            var syncDoctorCommand = new SyncDoctorCommand();
            await _mediator.Send(syncDoctorCommand);

            return Ok();
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Internal error");
            return StatusCode((int)HttpStatusCode.GatewayTimeout, $"Support code : {HttpContext.Connection.Id}");
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("syncDoc/background", Name = "syncDocBackground")]
    [SwaggerOperation(
        Summary = "Gets doc from a fake external data service as a Background task",
        Description = "No or default authentication required",
        OperationId = "SyncDocBackground",
        Tags = new[] { "EDCM" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult SyncDoctorsInBackground()
    {
        try
        {
            _iedcmSyncService.SyncDoctorsInBackground();
            return Ok();
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Internal error");
            return StatusCode((int)HttpStatusCode.GatewayTimeout, $"Support code : {HttpContext.Connection.Id}");
        }
    }
}