namespace CleanArchitecture.DDD.API.Controllers;

public class ValidationController : BaseAPIController
{
    public ValidationController(IAppServices appServices) : base(appServices)
    {
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
}