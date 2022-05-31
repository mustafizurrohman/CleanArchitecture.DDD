namespace CleanArchitecture.DDD.API.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public abstract class BaseAPIController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    protected DomainDbContext DbContext { get; } 
    
    /// <summary>
    /// 
    /// </summary>
    protected IMapper AutoMapper { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="autoMapper"></param>
    protected BaseAPIController(DomainDbContext dbContext, IMapper autoMapper)
    {
        DbContext = dbContext;
        AutoMapper = autoMapper;
    }
}