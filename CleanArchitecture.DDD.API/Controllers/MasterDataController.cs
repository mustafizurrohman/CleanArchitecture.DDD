
using CleanArchitecture.DDD.API.Controllers.BaseController;

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
    /// <param name="includeSoftDeleted"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("patient")]
    [SwaggerOperation(
        Summary = "Retrieves all patients from database ",
        Description = DefaultDescription,
        OperationId = "Get All Patients",
        Tags = new[] { "MasterData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Patient>))]
    public async Task<IActionResult> Patients(CancellationToken cancellationToken, bool includeSoftDeleted = false)
    {
        var patients = await DbContext.Patients
            .Where(p => p.MasterData.DateOfBirth > DateTime.Now.AddYears(-5))
            .Select(p => new { p.Fullname, p.MasterData.DateOfBirth})
            .ToListAsync(cancellationToken);

        return Ok(patients);

    }


}