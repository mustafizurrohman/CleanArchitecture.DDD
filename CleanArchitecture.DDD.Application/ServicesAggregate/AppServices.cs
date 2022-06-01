using Ardalis.GuardClauses;

namespace CleanArchitecture.DDD.Application.ServicesAggregate;

public class AppServices : IAppServices
{
    public DomainDbContext DbContext { get; }
    public IMapper AutoMapper { get; }
    public IMediator Mediator { get; }

    public AppServices(DomainDbContext dbContext, IMapper autoMapper, IMediator mediator)
    {
        DbContext = Guard.Against.Null(dbContext, nameof(dbContext));
        AutoMapper = Guard.Against.Null(autoMapper, nameof(autoMapper));
        Mediator = Guard.Against.Null(mediator, nameof(mediator));
    }
}