﻿using CleanArchitecture.DDD.Application.DTO.Internal;

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

    [HttpGet("", Name = "ScrutorDemoInjection")]
    [SwaggerOperation(
        Summary = "Demo of DI using scrutor",
        Description = "No or default authentication required",
        OperationId = "Test Scrutor",
        Tags = new[] { "DependencyInjection" }
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult TestScrutor([FromServices] ITestService testService)
    {
        return Ok(testService.HelloWorld());
    }

    [HttpGet("multiple", Name = "ScrutorDemoMultipleInjection")]
    [SwaggerOperation(
        Summary = "Demo of DI of multiple services using scrutor",
        Description = "No or default authentication required",
        OperationId = "Test Scrutor Multiple",
        Tags = new[] { "DependencyInjection" }
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult TestScrutorMultipleServices([FromServices] IEnumerable<ITestService> testServices)
    {
        var combinedString = testServices
            .Select(svc => svc.HelloWorld())
            .Aggregate((a, b) => a + Environment.NewLine + b);

        return Ok(combinedString);
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("decoration", Name = "ScrutorDemoDecoration")]
    [SwaggerOperation(
        Summary = "Demo of Decoration of services using scrutor",
        Description = "No or default authentication required",
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