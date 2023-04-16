using CleanArchitecture.DDD.API.Controllers.BaseController;

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

    private static IEnumerable<ExternalFakeDoctorAddressDTO> _cachedDTOsValid = new List<ExternalFakeDoctorAddressDTO>();
    private static IEnumerable<ExternalFakeDoctorAddressDTO> _cachedDTOsValidAndInvalid = new List<ExternalFakeDoctorAddressDTO>();
    private static int _iteration = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    /// <param name="fakeDataService"></param>
    public FakeController(IAppServices appServices, IFakeDataService fakeDataService) 
        : base(appServices)
    {
        _fakeDataService = Guard.Against.Null(fakeDataService);
    }

    [HttpGet("doctors")]
    [SwaggerOperation(
        Summary = "Generates fake doctors",
        Description = "No authentication required",
        OperationId = "GetFakeDoctorsWithSomeInvalidData",
        Tags = new[] { "FakeData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<ExternalFakeDoctorAddressDTO>))]
    public IActionResult GetFakeDoctors(int num = 10, CancellationToken cancellationToken = default)
    {
        Log.Information("Generating fake doctors ... ");

        // Simulate a fake delay here
        // Thread.Sleep(2000);

        // Simulate a fake error here- Fail every 2 out of 3 times
        if (++_attempts % 3 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);
        
        if (_cachedDTOsValid.Any())
        {
            return Ok(_cachedDTOsValid);
        }

        _cachedDTOsValid = _fakeDataService.GetValidDoctors(num);
        return Ok(_cachedDTOsValid);
    }

    [HttpGet("doctors/invalid")]
    [SwaggerOperation(
        Summary = "Generates fake doctors",
        Description = "No authentication required",
        OperationId = "GetFakeDoctorsWithSomeInvalidData",
        Tags = new[] { "FakeData" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Doctor was retrieved", typeof(IEnumerable<ExternalFakeDoctorAddressDTO>))]
    public IActionResult GetDoctorsWithInvalidData(int num = 10, CancellationToken cancellationToken = default)
    {
        Log.Information("Generating fake doctors where some of them are invalid ... ");

        // Simulate a fake delay here
        // Thread.Sleep(2000);

        // Simulate a fake error here- Fail every 2 out of 3 times
        if (++_attempts % 3 != 0)
            return StatusCode((int)HttpStatusCode.GatewayTimeout);

        if (_cachedDTOsValidAndInvalid.Any())
        {
            var updatedDoctors = _fakeDataService.GetDoctorsWithUpdatedAddress(_cachedDTOsValidAndInvalid, ++_iteration).ToList();
            updatedDoctors.AddRange(_fakeDataService.GetValidDoctors(_iteration));

            _cachedDTOsValidAndInvalid = updatedDoctors;

            return Ok(updatedDoctors);
        }

        _cachedDTOsValidAndInvalid = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num);
        return Ok(_cachedDTOsValidAndInvalid);
    }

}