using FakeDTO = CleanArchitecture.DDD.Application.DTO.FakeDoctorAddressDTO;

namespace CleanArchitecture.DDD.API.Controllers;

public class ValidationController : BaseAPIController
{
    private readonly IFakeDataService _fakeDataService;

    public ValidationController(IAppServices appServices, IFakeDataService fakeDataService) 
        : base(appServices)
    {
        _fakeDataService = fakeDataService;
    }

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
    public IActionResult DemoExtensionMethod(bool withModelError = false)
    {
        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(100).ToList();

        var doctorsToValidate = AutoMapper.Map<IEnumerable<ExternalFakeDoctorAddressDTO>, IEnumerable<FakeDoctorAddressDTO>>
            (fakeDoctors).ToList();

        var validationReport = doctorsToValidate
            .Where(doc => withModelError ? !doc.GetModelValidationReport().Valid : doc.GetModelValidationReport().Valid)
            .Select(doc => doc.GetModelValidationReport())
            .FirstOrDefault();
        
        return Ok(validationReport);
    }

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
    public async Task<IActionResult> DemoExtensionMethodForCollection([FromQuery] int num = 10)
    {
        num = Guard.Against.NegativeOrZero(num, nameof(num));

        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        var doctorsToValidate = AutoMapper.Map<IEnumerable<ExternalFakeDoctorAddressDTO>, IEnumerable<FakeDoctorAddressDTO>>
                (fakeDoctors);

        var validationReport = await doctorsToValidate.GetModelValidationReportAsync();

        var toDebugAsString = validationReport.ToFormattedJson();

        return Ok(validationReport);
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/error/")]
    [SwaggerOperation(
        Summary = "Demo of incorrect usage of Extension method",
        Description = DefaultDescription,
        OperationId = "Extension Method Validation Incorrect usage",
        Tags = new[] { "Validation" }
    )]
    public async Task<IActionResult> DemoExtensionMethodErrorForObject()
    {
        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(10).ToList();

        var doctorToValidate = fakeDoctors.First();

        // We have not defined a validator for ExternalFakeDoctorAddressDTO
        // So this  will throw an exception at runtime
        var validationReport = await doctorToValidate.GetModelValidationReportAsync();
        
        return Ok(validationReport);
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("demo/extensionMethod/error/collection")]
    [SwaggerOperation(
        Summary = "Demo of incorrect usage of Extension method for IEnumerable",
        Description = DefaultDescription,
        OperationId = "Extension Method IEnumerable Incorrect usage",
        Tags = new[] { "Validation" }
    )]
    public async Task<IActionResult> DemoExtensionMethodErrorForCollection()
    {
        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(10).ToList();
        
        // We have not defined a validator for ExternalFakeDoctorAddressDTO
        // So this  will throw an exception at runtime
        var validationReport = await fakeDoctors.GetModelValidationReportAsync();
        
        return Ok(validationReport);
    }

}
