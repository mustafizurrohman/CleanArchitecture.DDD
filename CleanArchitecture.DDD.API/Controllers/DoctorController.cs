using AutoMapper;

namespace CleanArchitecture.DDD.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class DoctorController : BaseAPIController
{
    public DoctorController(DomainDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {

    }
}