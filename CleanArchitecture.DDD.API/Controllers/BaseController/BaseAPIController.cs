namespace CleanArchitecture.DDD.API.Controllers.BaseController;

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
    protected IMediator Mediator { get; }

    /// <summary>
    /// 
    /// </summary>
    protected Faker Faker { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    protected BaseAPIController(IAppServices appServices)
    {
        DbContext = Guard.Against.Null(appServices.DbContext);
        AutoMapper = Guard.Against.Null(appServices.AutoMapper);
        Mediator = Guard.Against.Null(appServices.Mediator);
        Faker = Guard.Against.Null(appServices.Faker);
    }


}