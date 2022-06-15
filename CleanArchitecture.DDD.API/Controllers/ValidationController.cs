using CleanArchitecture.DDD.API.Controllers.Fake;
using CleanArchitecture.DDD.Application.ExtensionMethods;
using CleanArchitecture.DDD.Core.ExtensionMethods;

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
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("validation/extensionMethod")]
    [SwaggerOperation(
        Summary = "Demo of Extension method",
        Description = "No or default authentication required",
        OperationId = "Extension Method Validation",
        Tags = new[] { "Validation" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DemoExtensionMethod([FromBody] int num)
    {
        var fakeDoctors = _fakeDataService.GetFakeDoctorsWithSomeInvalidData(num);

        var validationReport = await fakeDoctors.GetModelValidationReportAsync();

        return Ok(validationReport.ToFormattedJson());
    }
}