using CleanArchitecture.DDD.API.Controllers.BaseController;
using Serilog.Context;

namespace CleanArchitecture.DDD.API.Controllers;

public class LoggingController : BaseAPIController
{
    private const string Tag = "Logging";

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
        Log.Information("[CORRECT] Param value is {randomParameter}", randomParameter);
        Log.Information($"[INCORRECT] Param value is {randomParameter}");

        // Incorrect with string interpolation. Refer SEQ entry
        Log.Information("[CORRECT] Param value is {param}", loggingParam);
        Log.Information($"[INCORRECT] Param value is {loggingParam}");
        
        return Ok();
    }

    /// <summary>
    /// Demo 2
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("scope", Name = "scope")]
    [SwaggerOperation(
        Summary = "Demo for logging using param",
        Description = DefaultDescription,
        OperationId = "Log Generation Demo with Parameter",
        Tags = new[] { "Logging" }
    )]
    public IActionResult LoggingScopeDemo(CancellationToken _)
    {
        var randomParameter = new Faker().Random.Word();

        var loggingParam = new
        {
            Word = randomParameter,
            Wordlength = randomParameter.Length
        };

        using (LogContext.PushProperty("ScopeID", "ScopeTest", true))
        {
            Log.Information("[CORRECT] Param value is {loggingParam}", loggingParam);
            Log.Information($"[INCORRECT] Param value is {randomParameter}");
        }

        return Ok();
    }

    /// <summary>
    /// Demo 3
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