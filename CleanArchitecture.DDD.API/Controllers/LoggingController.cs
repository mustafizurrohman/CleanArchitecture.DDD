using CleanArchitecture.DDD.Application.MediatR.Commands;
using System.Security.Cryptography;
using System.Threading;

namespace CleanArchitecture.DDD.API.Controllers;

public class LoggingController : BaseAPIController
{
    public LoggingController(IAppServices appServices)
        : base(appServices)
    {
            
    }

    /// <summary>
    /// Demo 1
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("param", Name = "param")]
    [SwaggerOperation(
        Summary = "Demo for logging using param",
        Description = DefaultDescription,
        OperationId = "Log Generation Demo with Parameter",
        Tags = new[] { "Logging" }
    )]
    public IActionResult LogDemo(CancellationToken cancellationToken, string? randomParameter)
    {
        if (string.IsNullOrWhiteSpace(randomParameter))
            randomParameter = new Faker().Random.Word();

        var loggingParam = new
        {
            Word = randomParameter,
            Wordlength = randomParameter.Length
        };

        // Correct. Refer SEQ entry
        Log.Information("Correct way of logging with parameter. Param value is {randomParameter}", randomParameter);
        Log.Information($"Incorrect way of logging with parameter. Param value is {randomParameter}");

        // Incorrect with string interpolation. Refer SEQ entry
        Log.Information("Correct way of logging with parameter. Param value is {param}", loggingParam);
        Log.Information($"Incorrect way of logging with parameter. Param value is {loggingParam}");
        
        return Ok();
    }

    /// <summary>
    /// Demo 2
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("traceability", Name = "traceability")]
    [SwaggerOperation(
        Summary = "Demo of exception logging and Traceability and Support Code",
        Description = DefaultDescription,
        OperationId = "Log Traceability",
        Tags = new[] { "Logging" }
    )]
    public async Task<IActionResult> TestExceptionLogging(CancellationToken cancellationToken)
    {
        var loggingCommand = new LoggingCommand();
        await Mediator.Send(loggingCommand, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Demo 3
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("generation", Name = "generation")]
    [SwaggerOperation(
        Summary = "Demo for Generation of logs for Seq Visualization",
        Description = DefaultDescription,
        OperationId = "Log Generation Demo",
        Tags = new[] { "Logging" }
    )]
    public async Task<IActionResult> LogGenerationDemo(CancellationToken cancellationToken, int iterations = 10, bool withDelay = true)
    {
        var generateLogsCommand = new GenerateLogsCommand(iterations, withDelay);
        await Mediator.Send(generateLogsCommand, cancellationToken);

        return Ok();
    }



}