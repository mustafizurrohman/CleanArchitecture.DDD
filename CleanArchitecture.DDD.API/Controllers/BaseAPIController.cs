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
    protected const string DefaultDescription = "No or default authentication required";

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    protected DomainDbContext DbContext { get; } 
    
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    protected IMapper AutoMapper { get; }

    /// <summary>
    /// 
    /// </summary>
    protected  IMediator Mediator { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    protected BaseAPIController(IAppServices appServices)
    {
        DbContext = Guard.Against.Null(appServices.DbContext, nameof(appServices));
        AutoMapper = Guard.Against.Null(appServices.AutoMapper, nameof(appServices));
        Mediator = Guard.Against.Null(appServices.Mediator, nameof(appServices));
    }
    

}