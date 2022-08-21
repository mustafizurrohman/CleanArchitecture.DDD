using CleanArchitecture.DDD.Application.MediatR.Commands;
using System.Security.Cryptography;
using System.Threading;

namespace CleanArchitecture.DDD.API.Controllers;

public class LoggingController : BaseAPIController
{
    private int RandomDelay => RandomNumberGenerator.GetInt32(750, 1000);

    public LoggingController(IAppServices appServices)
        : base(appServices)
    {
            
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("traceability", Name = "traceability")]
    [SwaggerOperation(
        Summary = "Demo of exception logging and Traceability and support code",
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
    /// 
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