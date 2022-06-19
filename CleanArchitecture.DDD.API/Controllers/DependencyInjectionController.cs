namespace CleanArchitecture.DDD.API.Controllers;

public class DependencyInjectionController : BaseAPIController
{
    /// <summary>
    /// Demonstrates features of Dependency Injection where Services are injected
    /// using an Extension Method which uses Scrutor
    /// We are skipping the use of MediatR here for the sake of simplicity
    /// </summary>
    /// <param name="appServices"></param>
    public DependencyInjectionController(IAppServices appServices) 
        : base(appServices)
    {
    }

    [HttpGet("scrutor", Name = "ScrutorTest")]
    [SwaggerOperation(
        Summary = "Demo of DI using scrutor",
        Description = "No or default authentication required",
        OperationId = "Test Scrutor",
        Tags = new[] { "DependencyInjection" }
    )]
    public IActionResult TestScrutor([FromServices] ITestService testService)
    {
        return Ok(testService.HelloWorld());
    }

    [HttpGet("scrutor/multiple", Name = "ScrutorTestMultiple")]
    [SwaggerOperation(
        Summary = "Demo of DI of multiple services using scrutor",
        Description = "No or default authentication required",
        OperationId = "Test Scrutor Multiple",
        Tags = new[] { "DependencyInjection" }
    )]
    public IActionResult TestScrutorMultipleServices([FromServices] IEnumerable<ITestService> testServices)
    {
        var combinedString = string.Empty;

        foreach (var testService in testServices)
        {
            combinedString += testService.HelloWorld() + Environment.NewLine;
        }

        return Ok(combinedString);
    }

}