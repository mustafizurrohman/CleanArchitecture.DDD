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
    private static IEnumerable<FakeDoctorAddressDTO> _cachedDTOs = new List<FakeDoctorAddressDTO>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    /// <param name="fakeDataService"></param>
    public FakeController(IAppServices appServices, IFakeDataService fakeDataService) 
        : base(appServices)
    {
        _fakeDataService = Guard.Against.Null(fakeDataService, nameof(fakeDataService));
    }

    [HttpGet("doctors")]
    [SwaggerOperation(
        Summary = "Generates fake doctors",
        Description = "No authentication required",
        OperationId = "GetFakeDoctors",
        Tags = new[] { "FakeData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Doctor>))]
    public IActionResult GetFakeDoctors(int num = 10, CancellationToken cancellationToken = default)
    {
        // Simulate a fake delay here
        // Thread.Sleep(2000);

        // Simulate a fake error here- Fail every 2 out of 3 times
        if (++_attempts % 3 != 0)
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

    [HttpGet("doctors/invalid")]
    [SwaggerOperation(
        Summary = "Generates fake doctors",
        Description = "No authentication required",
        OperationId = "GetFakeDoctors",
        Tags = new[] { "FakeData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<Doctor>))]
    public IActionResult GetDoctorsWithInvalidData(int num = 10, CancellationToken cancellationToken = default)
    {
        // Simulate a fake delay here
        // Thread.Sleep(2000);

        // Simulate a fake error here- Fail every 2 out of 3 times
        if (++_attempts % 3 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);

        if (_cachedDTOs.Any())
            return Ok(_cachedDTOs);
        
        _cachedDTOs = _fakeDataService.GetFakeDoctors(num);
        return Ok(_cachedDTOs);
    }

}