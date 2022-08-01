using System.Security.Cryptography;

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
    public IActionResult TestExceptionLogging()
    {
        var faker = new Faker();

        Thread.Sleep(RandomDelay);
        Log.Information("Testing if logs are co-related. {param} is a random parameter", faker.Lorem.Word());

        Thread.Sleep(RandomDelay);
        Log.Verbose("A verbose log message");

        Thread.Sleep(RandomDelay);
        Log.Debug("Here is a debug message with param {message}", "debug message param");

        Thread.Sleep(RandomDelay);
        Log.Fatal("This should actually never happen {param}", faker.Lorem.Sentence());

        Thread.Sleep(RandomDelay);
        Log.Error("Logging before throwing an exception .... ");

        Thread.Sleep(RandomDelay);
        throw new NotImplementedException();
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
    public async Task<IActionResult> LogGenerationDemo(CancellationToken cancellationToken, int iterations = 10, bool withDelay = false)
    {
        var generateLogsCommand = new GenerateLogsCommand(iterations, withDelay);
        await Mediator.Send(generateLogsCommand, cancellationToken);

        return Ok();
    }
}