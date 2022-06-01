namespace CleanArchitecture.DDD.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class DoctorController : BaseAPIController
{
    public DoctorController(IAppServices appServices)
        : base(appServices)
    {

    }
}