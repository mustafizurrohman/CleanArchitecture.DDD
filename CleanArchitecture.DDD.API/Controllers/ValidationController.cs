using CleanArchitecture.DDD.API.Controllers.Fake;
using CleanArchitecture.DDD.Application.DTO;
using CleanArchitecture.DDD.Application.ExtensionMethods;
using CleanArchitecture.DDD.Core.ExtensionMethods;
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
    [HttpPost("ValueObject/validation", Name = "valueObjectValidation")]
    [SwaggerOperation(
        Summary = "Demo of validation of Value Object",
        Description = "No or default authentication required",
        OperationId = "Validate Value Object",
        Tags = new[] { "Validation" }
    )]
    public IActionResult TestNameValueObject(string name)
    {
        var createdName = NameValueObject.Create(name);

        if (createdName.Error is not null)
            return BadRequest(createdName.Error.Message);

        return Ok(createdName.Value);
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("ValueObect/validation/fluentValidationPipeline")]
    [SwaggerOperation(
        Summary = "Demo of input validation using FluentValidation",
        Description = "No or default authentication required",
        OperationId = "Input Validation",
        Tags = new[] { "Validation" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateName([FromBody] Name name)
    {
        return Ok();
    }

    // TODO: Debug and fix this!
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("validation/extensionMethod")]
    [SwaggerOperation(
        Summary = "Demo of Extension method",
        Description = "No or default authentication required",
        OperationId = "Extension Method Validation",
        Tags = new[] { "Validation" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DemoExtensionMethod([FromBody] int num = 10)
    {
        List<ExternalFakeDoctorAddressDTO> fakeDoctors =
            _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num).ToList();

        var doctorsToValidate =
            AutoMapper.Map<IEnumerable<ExternalFakeDoctorAddressDTO>, IEnumerable<FakeDoctorAddressDTO>>(fakeDoctors);

        var validationReport = await doctorsToValidate.GetModelValidationReportAsync();

        return Ok(validationReport.ToFormattedJson());
    }
}