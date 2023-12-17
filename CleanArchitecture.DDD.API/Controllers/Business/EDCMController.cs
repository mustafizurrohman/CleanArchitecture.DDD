using CleanArchitecture.DDD.Application.MediatR.Commands.SyncDoctor;

namespace CleanArchitecture.DDD.API.Controllers.Business;

/// <summary>
/// 
/// </summary>
/// <param name="appServices"></param>
/// <param name="iedcmSyncService"></param>
public class EDCMController(IAppServices appServices, IEDCMSyncService iedcmSyncService) 
    : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "EDCM";


    /// <summary>
    /// 
    /// </summary>
    /// <param name="simulateError"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("syncDoc", Name = "Sync Doctors")]
    [SwaggerOperation(
        Summary = "Gets doc from a fake external data service",
        Description = DefaultDescription,
        OperationId = "Sync Doctors",
        Tags = [DefaultControllerTag]
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<IActionResult> SyncDoctors(bool simulateError, CancellationToken cancellationToken)
    {
        try
        {
            var syncDoctorCommand = new SyncDoctorCommand(simulateError);
            await Mediator.Send(syncDoctorCommand, cancellationToken);

            return Ok();
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex.Demystify(), "Internal error");
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
        Description = DefaultDescription,
        OperationId = "SyncDocBackground",
        Tags = [DefaultControllerTag]
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult SyncDoctorsInBackground()
    {
        try
        {
            iedcmSyncService.SyncDoctorsInBackground();
            return Ok();
        }
        catch (HttpRequestException ex)
        {
            Log.Error(ex.Demystify(), "Internal error");
            return StatusCode((int)HttpStatusCode.GatewayTimeout, $"Support code : {HttpContext.GetSupportCode()}");
        }
    }
}