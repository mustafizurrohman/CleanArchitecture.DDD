using CleanArchitecture.DDD.Application.DTO.Internal;
using CleanArchitecture.DDD.Core.Attributes;

namespace CleanArchitecture.DDD.API.Controllers;

[Route("scrutor")]
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="testService"></param>
    /// <returns></returns>
    [HttpGet("", Name = "ScrutorDemoInjection")]
    [SwaggerOperation(
        Summary = "Demo of DI using scrutor",
        Description = DefaultDescription,
        OperationId = "Test Scrutor",
        Tags = new[] { "DependencyInjection" }
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult TestScrutor([FromServices] ITestService testService)
    {
        return Ok(testService.HelloWorld());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="testServices"></param>
    /// <returns></returns>
    [HttpGet("multiple", Name = "ScrutorDemoMultipleInjection")]
    [SwaggerOperation(
        Summary = "Demo of DI of multiple services using scrutor",
        Description = DefaultDescription,
        OperationId = "Test Scrutor Multiple",
        Tags = new[] { "DependencyInjection" }
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult TestScrutorMultipleServices([FromServices] IEnumerable<ITestService> testServices)
    {
        var serviceOutputs = testServices
            .OrderBy(svc => svc.GetType().GetCustomAttribute<InjectionOrderAttribute>()?.Order ?? 0)
            .Select(svc => new
            {
                ServiceType = svc.GetType().Name, 
                ServiceOutput = svc.HelloWorld()
            })
            .ToList();


        return Ok(serviceOutputs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataService"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("decoration", Name = "ScrutorDemoDecoration")]
    [SwaggerOperation(
        Summary = "Demo of Decoration of services using scrutor",
        Description = DefaultDescription,
        OperationId = "Test Scrutor Decoration",
        Tags = new[] { "DependencyInjection" }
    )]
    [ProducesResponseType(typeof(IEnumerable<DemoData>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TestScrutorDecoration([FromServices] IDataService dataService)
    {
        var data = await dataService.GetDemoDataAsync(10);
        return Ok(data);
    }
}