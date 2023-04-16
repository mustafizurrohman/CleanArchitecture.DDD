using CleanArchitecture.DDD.API.Controllers.BaseController;

namespace CleanArchitecture.DDD.API.Controllers.EntityFramework;

public class MasterDataController : BaseAPIController
{

    public MasterDataController(IAppServices appServices)
        : base(appServices)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="includeSoftDeleted"></param>
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
    public async Task<IActionResult> GetAllDoctors(CancellationToken cancellationToken, bool includeSoftDeleted = false)
    {
        var command = new GetAllDoctorsQuery(includeSoftDeleted);
        var result = (await Mediator.Send(command, cancellationToken)).ToList();

        var response = new
        {
            Doctors = result,
            result.Count
        };

        return Ok(response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="years"></param>
    /// <param name="includeSoftDeleted"></param>
    /// <returns></returns>
    [HttpGet("patient")]
    [SwaggerOperation(
        Summary = "Retrieves all patients from database younger than specified number of years",
        Description = DefaultDescription,
        OperationId = "Get All Patients",
        Tags = new[] { "MasterData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Young patients was retrieved", typeof(IEnumerable<Patient>))]
    public async Task<IActionResult> Patients(CancellationToken cancellationToken, int years, bool includeSoftDeleted = false)
    {
        var query = new GetYoungPatientsQuery(years, includeSoftDeleted);

        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);

    }


}