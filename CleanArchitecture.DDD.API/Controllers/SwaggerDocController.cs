using AutoMapper;
using CleanArchitecture.DDD.Domain.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.DDD.API.Controllers;

[ApiExplorerSettings(IgnoreApi = false)]
public class SwaggerDocController : BaseAPIController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="autoMapper"></param>
    public SwaggerDocController(DomainDbContext dbContext, IMapper autoMapper)
        : base(dbContext, autoMapper)
    {
    }

    [HttpGet("doctors")]
    [SwaggerOperation(
        Summary = "Retrieves all doctors from database",
        Description = "No authentication required",
        OperationId = "GetAllDoctors",
        Tags = new[] { "Doctors" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Doctor>))]
    public async Task<IActionResult> GetAllDoctors(CancellationToken cancellationToken)
    {
        var doctors = await DbContext.Doctors.ToListAsync(cancellationToken);
        return Ok(doctors);
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
        Tags = new[] { "Doctors" }
    )]
    [ProducesResponseType(typeof(IEnumerable<DoctorCityDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchVersion2(
        [FromQuery, SwaggerParameter("Search keyword- Firstname", Required = false)] string? firstName,
        [FromQuery, SwaggerParameter("Search keyword- Middlename", Required = false)] string? middleName,
        [FromQuery, SwaggerParameter("Search keyword- Lastname", Required = false)] string? lastName,
        CancellationToken cancellationToken)
    {
        try
        {
            firstName = firstName?.Trim() ?? string.Empty;
            middleName = middleName?.Trim() ?? string.Empty;
            lastName = lastName?.Trim() ?? string.Empty;

            var query = DbContext.Doctors.AsQueryable();

            var name = Name.Create(firstName, middleName, lastName);

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(doc => doc.Name.Firstname.ToLower().Contains(name.Firstname.ToLower()));

            if (!string.IsNullOrWhiteSpace(middleName)) {

                #pragma warning disable CS8602 
                // Dereference of a possibly null reference.
                query = query
                    .Where(doc => doc.Name.Middlename != null)
                    .Where(doc => doc.Name.Middlename.ToLower().Contains(name.Middlename.ToLower()));
                #pragma warning restore CS8602 
                // Dereference of a possibly null reference.
            }

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(doc => doc.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));

            var results = await query.AsNoTracking()
                .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Ok(results);
        }
        catch (DomainValidationException ex)
        {
            Log.Information(ex, "Smart optimization");

            // If an Exception of this type of thrown it means that the 
            // search query does not represent a valid domain model and in that case we do not need
            // to search the database at all
            return Ok(Enumerable.Empty<DoctorCityDTO>());
        }
    }

}
