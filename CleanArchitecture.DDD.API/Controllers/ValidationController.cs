namespace CleanArchitecture.DDD.API.Controllers;

public class ValidationController : BaseAPIController
{
    private readonly IFakeDataService _fakeDataService;

    public ValidationController(IAppServices appServices, IFakeDataService fakeDataService) 
        : base(appServices)
    {
        _fakeDataService = fakeDataService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("ValueObject", Name = "valueObjectValidation")]
    [SwaggerOperation(
        Summary = "Demo of validation of Value Object",
        Description = DefaultDescription,
        OperationId = "Validate Value Object",
        Tags = new[] { "Validation" }
    )]
    public IActionResult TestNameValueObject(string name)
    {
        var createdName = NameValueObject.Create(name);
        
        if (createdName.IsFailure)
            return BadRequest(createdName.Error.Message);
        
        return Ok(createdName.Value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("ValueObject/fluentValidationPipeline")]
    [SwaggerOperation(
        Summary = "Demo of input validation using FluentValidation",
        Description = DefaultDescription,
        OperationId = "Input Validation",
        Tags = new[] { "Validation" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateName([FromBody] Name name)
    {
        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="withModelError"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod")]
    [SwaggerOperation(
        Summary = "Demo of Extension method for a single object",
        Description = DefaultDescription,
        OperationId = "Extension Method Validation",
        Tags = new[] { "Validation" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DemoExtensionMethod(bool withModelError = false, int num = 100)
    {
        if (num <= 0)
            return BadRequest(nameof(num) + " must be positive");

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        var doctorsToValidate = AutoMapper.Map<IEnumerable<ExternalFakeDoctorAddressDTO>, IEnumerable<FakeDoctorAddressDTO>>
            (fakeDoctors).ToList();

        var validationReport = doctorsToValidate
            .Where(doc => withModelError ? !doc.GetModelValidationReport().Valid : doc.GetModelValidationReport().Valid)
            .Select(doc => doc.GetModelValidationReport())
            .FirstOrDefault();
        
        return Ok(validationReport);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/collection")]
    [SwaggerOperation(
        Summary = "Demo of Extension method for IEnumerable",
        Description = DefaultDescription,
        OperationId = "Extension Method IEnumerable Validation",
        Tags = new[] { "Validation" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DemoExtensionMethodForCollection(int num = 100)
    {
        if (num <= 0)
            return BadRequest(nameof(num) + " must be positive");

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        var doctorsToValidate = AutoMapper.Map<IEnumerable<ExternalFakeDoctorAddressDTO>, IEnumerable<FakeDoctorAddressDTO>>
                (fakeDoctors);

        var validationReport = await doctorsToValidate.GetModelValidationReportAsync();

        var validationReportAsJsonString = validationReport.ToFormattedJson();

        return Ok(validationReport);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/error/")]
    [SwaggerOperation(
        Summary = "Demo of incorrect usage of Extension method",
        Description = DefaultDescription,
        OperationId = "Extension Method Validation Incorrect usage",
        Tags = new[] { "Validation" }
    )]
    public async Task<IActionResult> DemoExtensionMethodErrorForObject(int num = 100)
    {
        if (num <= 0)
            return BadRequest(nameof(num) + " must be positive");

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        var doctorToValidate = fakeDoctors.First();

        // We have not defined a validator for ExternalFakeDoctorAddressDTO
        // So this  will throw an exception at runtime
        var validationReport = await doctorToValidate.GetModelValidationReportAsync();
        
        return Ok(validationReport);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/error/collection")]
    [SwaggerOperation(
        Summary = "Demo of incorrect usage of Extension method for IEnumerable",
        Description = DefaultDescription,
        OperationId = "Extension Method IEnumerable Incorrect usage",
        Tags = new[] { "Validation" }
    )]
    public async Task<IActionResult> DemoExtensionMethodErrorForCollection(int num = 100)
    {
        if (num <= 0)
            return BadRequest(nameof(num) + " must be positive");

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();
        
        // We have not defined a validator for ExternalFakeDoctorAddressDTO
        // So this  will throw an exception at runtime
        var validationReport = await fakeDoctors.GetModelValidationReportAsync();
        
        return Ok(validationReport);
    }

}
