using AutoMapper;

namespace CleanArchitecture.DDD.API.Controllers;

public class DoctorController : BaseAPIController
{
    public DoctorController(DomainDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {

    }
}