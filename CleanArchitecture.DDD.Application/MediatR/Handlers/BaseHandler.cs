namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public abstract class BaseHandler
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
    protected IMediator Mediator { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    protected BaseHandler(IAppServices appServices)
    {
        DbContext = Guard.Against.Null(appServices.DbContext);
        AutoMapper = Guard.Against.Null(appServices.AutoMapper);
        Mediator = Guard.Against.Null(appServices.Mediator);
    }
}