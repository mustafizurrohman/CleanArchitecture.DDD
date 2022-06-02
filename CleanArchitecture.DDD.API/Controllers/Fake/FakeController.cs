using System.Net;
using CleanArchitecture.DDD.Application.DTO;

namespace CleanArchitecture.DDD.API.Controllers.Fake;

/// <summary>
/// Fake Controller
/// In a real application this will be another service
/// like CRM
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class FakeController : BaseAPIController
{
    private readonly IFakeDataService _fakeDataService;

    private static int _attempts = 0;
    private static IEnumerable<DoctorDTO> _cachedDoctors = new List<DoctorDTO>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    /// <param name="fakeDataService"></param>
    public FakeController(IAppServices appServices, IFakeDataService fakeDataService) 
        : base(appServices)
    {
        _fakeDataService = fakeDataService;
    }

    [HttpGet("doctors")]
    [SwaggerOperation(
        Summary = "Retrieves all doctors from database",
        Description = "No authentication required",
        OperationId = "GetAllDoctors",
        Tags = new[] { "FakeData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Doctor>))]
    public IActionResult GetFakeDoctors(int num = 10, CancellationToken cancellationToken = default)
    {
        // Simulate a fake delay here
        Thread.Sleep(2000);

        // Simulate a fake error here
        if (++_attempts % 2 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);
        
        if (_cachedDoctors.Any())
        {
            var updatedDoctors = _fakeDataService.GetDoctorsWithUpdatedAddress(_cachedDoctors).ToList();
            updatedDoctors.AddRange(_fakeDataService.GetDoctors(1));
            
            return Ok(updatedDoctors);
        }


        _cachedDoctors = _fakeDataService.GetDoctors(num);
        return Ok(_cachedDoctors);
    }
    
}