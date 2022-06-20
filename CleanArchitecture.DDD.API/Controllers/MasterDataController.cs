namespace CleanArchitecture.DDD.API.Controllers;

public class MasterDataController : BaseAPIController
{

    public MasterDataController(IAppServices appServices) 
        : base(appServices)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("doctors")]
    [SwaggerOperation(
        Summary = "Retrieves all doctors from database",
        Description = DefaultDescription,
        OperationId = "Get All Doctors",
        Tags = new[] { "MasterData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Doctor>))]
    public async Task<IActionResult> GetAllDoctors(CancellationToken cancellationToken)
    {
        var command = new GetAllDoctorsQuery();
        var result = await Mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    
}