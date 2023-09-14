using CleanArchitecture.DDD.Application.DTO.Internal;
using CleanArchitecture.DDD.Core.Attributes;
using CleanArchitecture.DDD.Core.GuardClauses;

namespace CleanArchitecture.DDD.API.Controllers;

/// <summary>
/// Demonstrates features of Dependency Injection where Services are injected
/// using an Extension Method which uses Scrutor
/// We are skipping the use of MediatR here for the sake of simplicity
/// </summary>
/// <param name="appServices"></param>
[Route("scrutor")]
public class DependencyInjectionController(IAppServices appServices) 
    : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "DependencyInjection";

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
        Tags = new[] { DefaultControllerTag }
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
    /// <param name="descending"></param>
    /// <returns></returns>
    [HttpGet("multiple", Name = "ScrutorDemoMultipleInjection")]
    [SwaggerOperation(
        Summary = "Demo of DI of multiple services using scrutor",
        Description = DefaultDescription,
        OperationId = "Test Scrutor Multiple",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult TestScrutorMultipleServices([FromServices] IEnumerable<ITestService> testServices, bool descending)
    {
        var injectedServices = testServices.ToList();

        Guard.Against.EmptyOrNullEnumerable(injectedServices);
        
        var services = descending
            ? injectedServices.OrderByDescending(svc => svc.GetType().GetCustomAttribute<InjectionOrderAttribute>()?.Order ?? 0)
            : injectedServices.OrderBy(svc => svc.GetType().GetCustomAttribute<InjectionOrderAttribute>()?.Order ?? 0);

        var serviceOutputs = services
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
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(typeof(IEnumerable<DemoData>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TestScrutorDecoration([FromServices] IDataService dataService)
    {
        var data = await dataService.GetDemoDataAsync(10);
        return Ok(data);
    }
}
