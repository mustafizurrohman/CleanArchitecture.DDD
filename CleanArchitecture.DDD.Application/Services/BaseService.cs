namespace CleanArchitecture.DDD.Application.Services;

public class BaseService
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
    /// <param name="appServices"></param>
    protected BaseService(IAppServices appServices)
    {
        DbContext = Guard.Against.Null(appServices.DbContext, nameof(appServices));
        AutoMapper = Guard.Against.Null(appServices.AutoMapper, nameof(appServices));
    }
}