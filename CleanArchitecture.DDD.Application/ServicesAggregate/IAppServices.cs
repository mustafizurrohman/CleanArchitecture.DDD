namespace CleanArchitecture.DDD.Application.ServicesAggregate;

public interface IAppServices
{
    public DomainDbContext DbContext { get; }

    public IMapper AutoMapper { get; }

    public IMediator Mediator { get; }
}