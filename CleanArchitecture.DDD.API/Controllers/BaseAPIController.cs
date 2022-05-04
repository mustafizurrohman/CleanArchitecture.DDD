using AutoMapper;

namespace CleanArchitecture.DDD.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    // ReSharper disable once InconsistentNaming
    public abstract class BaseAPIController : ControllerBase
    {
        protected DomainDbContext DbContext { get; } 
        protected IMapper AutoMapper { get; }

        protected BaseAPIController(DomainDbContext dbContext, IMapper autoMapper)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
        }
    }
}
