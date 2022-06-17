namespace CleanArchitecture.DDD.API.Controllers;

[ApiExplorerSettings(IgnoreApi = false)]
public class DemoController : BaseAPIController
{
    public DemoController(IAppServices appServices)
        : base(appServices)
    {
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("ValueObject/equality", Name = "valueObjectEquality")]
    [SwaggerOperation(
        Summary = "Demo of equality of Value Object",
        Description = "No or default authentication required",
        OperationId = "Check Value Object for equality",
        Tags = new[] { "Demo" }
    )]
    public IActionResult TestNameValueObjectForEquality(string name)
    {
        var createdName1 = NameValueObject.Create(name);
        var createdName2 = NameValueObject.Create(name);

        var name1 = createdName1.Value;
        var name2 = createdName2.Value;

        return Ok(name1 == name2);
    }



    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("exception", Name = "loggingException")]
    [SwaggerOperation(
        Summary = "Demo of exception logging and support code",
        Description = "No or default authentication required",
        OperationId = "Log Exception",
        Tags = new[] { "Demo" }
    )]
    public IActionResult TestExceptionLogging()
    {
        throw new NotImplementedException();
    }



}