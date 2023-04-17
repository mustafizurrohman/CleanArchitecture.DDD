namespace CleanArchitecture.DDD.API.Controllers.EntityFramework;

public class MasterDataController : BaseAPIController
{
    private const string DefaultControllerTag = "MasterData";

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
        Tags = new[] { DefaultControllerTag }
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
        OperationId = "Get All Young Patients",
        Tags = new[] { DefaultControllerTag }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Young patients was retrieved", typeof(IEnumerable<Patient>))]
    public async Task<IActionResult> Patients(CancellationToken cancellationToken, int years, bool includeSoftDeleted = false)
    {
        var query = new GetYoungPatientsQuery(years, includeSoftDeleted);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="patientId"></param>
    /// <returns></returns>
    [HttpPut("patient/inactivate")]
    [SwaggerOperation(
        Summary = "Inactivate an existing patient",
        Description = DefaultDescription,
        OperationId = "Inactivate Patient",
        Tags = new[] { DefaultControllerTag }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Patient was inactivated")]
    public async Task<IActionResult> InactivatePatient(CancellationToken cancellationToken, Guid patientId)
    {
        var query = new InactivatePatientCommand(patientId);
        await Mediator.Send(query, cancellationToken);

        return Ok();
    }


}