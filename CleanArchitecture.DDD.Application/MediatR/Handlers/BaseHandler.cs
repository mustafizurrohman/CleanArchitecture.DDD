using Ardalis.GuardClauses;
using CleanArchitecture.DDD.Application.ServicesAggregate;

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
        DbContext = Guard.Against.Null(appServices.DbContext, nameof(appServices));
        AutoMapper = Guard.Against.Null(appServices.AutoMapper, nameof(appServices));
        Mediator = Guard.Against.Null(appServices.Mediator, nameof(appServices));
    }
}