using CleanArchitecture.DDD.Application.MediatR.Queries;
using CleanArchitecture.DDD.Application.ServicesAggregate;
using CleanArchitecture.DDD.Domain.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

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
    /// <param name="firstName">Firstname</param>
    /// <param name="middleName">Middlename</param>
    /// <param name="lastName">Lastname</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Searches for doctors from database",
        Description = "No or default authentication required",
        OperationId = "SearchDoctorsVersion2",
        Tags = new[] { "Search" }
    )]
    [ProducesResponseType(typeof(IEnumerable<DoctorCityDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchVersion2(
        [FromQuery, SwaggerParameter("Search keyword- Firstname", Required = false)] string? firstName,
        [FromQuery, SwaggerParameter("Search keyword- Middlename", Required = false)] string? middleName,
        [FromQuery, SwaggerParameter("Search keyword- Lastname", Required = false)] string? lastName,
        CancellationToken cancellationToken)
    {
        var query = new SearchDoctorsQuery(firstName, middleName, lastName);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

}
