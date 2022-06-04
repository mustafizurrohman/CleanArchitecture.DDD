using System.Net;
using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.API.Controllers;

public class EDCMController : BaseAPIController
{
    private readonly IEDCMSyncService _iedcmSyncService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    /// <param name="iedcmSyncService"></param>
    public EDCMController(IAppServices appServices, IEDCMSyncService iedcmSyncService) 
        : base(appServices)
    {
        _iedcmSyncService = iedcmSyncService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="simulateError"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("syncDoc", Name = "Sync Doctors")]
    [SwaggerOperation(
        Summary = "Gets doc from a fake external data service",
        Description = "No or default authentication required",
        OperationId = "Sync Doctors",
        Tags = new[] { "EDCM" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<IActionResult> SyncDoctors(bool simulateError)
    {
        try
        {
            var syncDoctorCommand = new SyncDoctorCommand(simulateError);
            await Mediator.Send(syncDoctorCommand);

            return Ok();
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex, "Internal error");
            return StatusCode((int)HttpStatusCode.GatewayTimeout, $"Support code : {HttpContext.GetSupportCode()}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
            return StatusCode((int)HttpStatusCode.GatewayTimeout, $"Support code : {HttpContext.GetSupportCode()}");
        }
    }
}