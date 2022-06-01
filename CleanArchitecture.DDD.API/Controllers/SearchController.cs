namespace CleanArchitecture.DDD.API.Controllers;

[ApiExplorerSettings(IgnoreApi = false)]
public class SearchController : BaseAPIController
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    public SearchController(IAppServices appServices)
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
        Description = "No or default authentication required",
        OperationId = "GetAllDoctors",
        Tags = new[] { "Search" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Doctor>))]
    public async Task<IActionResult> GetAllDoctors(CancellationToken cancellationToken)
    {
        var command = new GetAllDoctorsQuery();
        var result = await Mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Search for doctors by name
    /// </summary>
    /// <param name="firstName">Firstname to search for</param>
    /// <param name="middleName">Middlename to search for</param>
    /// <param name="lastName">Lastname to search for</param>
    /// <param name="and"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Searches for doctors from database",
        Description = "No or default authentication required",
        OperationId = "SearchDoctors",
        Tags = new[] { "Search" }
    )]
    [ProducesResponseType(typeof(IEnumerable<DoctorCityDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchVersion2(
        CancellationToken cancellationToken,
        [FromQuery, SwaggerParameter("Search keyword- Firstname", Required = false)] string? firstName,
        [FromQuery, SwaggerParameter("Search keyword- Lastname", Required = false)] string? lastName,
        [FromQuery, SwaggerParameter("Search Criteria- And", Required = true)] bool and = true
        )
    {
        var query = new SearchDoctorsQuery(firstName, lastName, and);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

}
