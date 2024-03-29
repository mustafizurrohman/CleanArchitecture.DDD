﻿using CleanArchitecture.DDD.Application.MediatR.Queries.SearchDoctors;

namespace CleanArchitecture.DDD.API.Controllers.Business;

/// <summary>
/// 
/// </summary>
/// <param name="appServices"></param>
[ApiExplorerSettings(IgnoreApi = false)]
public class SearchController(IAppServices appServices) 
    : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "Search";
    
    /// <summary>
    /// Search for doctors by name
    /// </summary>
    /// <param name="firstName">Firstname to search for</param>
    /// <param name="lastName">Lastname to search for</param>
    /// <param name="and"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    [HttpGet("doctor")]
    [SwaggerOperation(
        Summary = "Searches for doctors from database",
        Description = DefaultDescription,
        OperationId = "Search Doctors",
        Tags = [DefaultControllerTag]
    )]
    [ProducesResponseType(typeof(IEnumerable<DoctorCityDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchVersion2(
        CancellationToken cancellationToken,
        [FromQuery, SwaggerParameter("Search keyword- Firstname", Required = true)] string? firstName,
        [FromQuery, SwaggerParameter("Search keyword- Lastname", Required = true)] string? lastName,
        [FromQuery, SwaggerParameter("Search Criteria- And", Required = true)] bool and = true
        )
    {
        var query = new SearchDoctorsQuery(firstName, lastName, and);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

}
