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

    private static IEnumerable<FakeDoctorAddressDTO> _cachedDTOsValid = new List<FakeDoctorAddressDTO>();
    private static IEnumerable<FakeDoctorAddressDTO> _cachedDTOsValidAndInvalid = new List<FakeDoctorAddressDTO>();
    private static int _iteration = 0;

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
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<FakeDoctorAddressDTO>))]
    public IActionResult GetFakeDoctors(int num = 10, CancellationToken cancellationToken = default)
    {
        // Simulate a fake delay here
        // Thread.Sleep(2000);

        // Simulate a fake error here- Fail every 2 out of 3 times
        if (++_attempts % 3 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);
        
        if (_cachedDTOsValid.Any())
        {
            return Ok(_cachedDTOsValid);
        }

        _cachedDTOsValid = _fakeDataService.GetDoctors(num);
        return Ok(_cachedDTOsValid);
    }

    [HttpGet("doctors/invalid")]
    [SwaggerOperation(
        Summary = "Generates fake doctors",
        Description = "No authentication required",
        OperationId = "GetFakeDoctors",
        Tags = new[] { "FakeData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<FakeDoctorAddressDTO>))]
    public IActionResult GetDoctorsWithInvalidData(int num = 10, CancellationToken cancellationToken = default)
    {
        // Simulate a fake delay here
        // Thread.Sleep(2000);

        // Simulate a fake error here- Fail every 2 out of 3 times
        if (++_attempts % 3 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);

        if (_cachedDTOsValidAndInvalid.Any())
        {
            var updatedDoctors = _fakeDataService.GetDoctorsWithUpdatedAddress(_cachedDTOsValidAndInvalid, ++_iteration).ToList();
            updatedDoctors.AddRange(_fakeDataService.GetDoctors(_iteration));

            _cachedDTOsValidAndInvalid = updatedDoctors;

            return Ok(updatedDoctors);
        }

        _cachedDTOsValidAndInvalid = _fakeDataService.GetFakeDoctors(num);
        return Ok(_cachedDTOsValidAndInvalid);
    }

}