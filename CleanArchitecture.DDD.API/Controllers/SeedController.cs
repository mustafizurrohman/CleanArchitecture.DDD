using CleanArchitecture.DDD.Application.Exceptions;

namespace CleanArchitecture.DDD.API.Controllers;

/// <summary>
/// 
/// </summary>
[ApiExplorerSettings(IgnoreApi = false)]
public class SeedController : BaseAPIController
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appServices"></param>
    public SeedController(IAppServices appServices)
        : base(appServices)
    {
    }

    /// <summary>
    /// Seed specified number of address in database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [HttpPost("address", Name = "seedAddress")]
    [SwaggerOperation(
        Summary = "Seed specified number of address in database",
        Description = "No or default authentication required",
        OperationId = "Seed Addresses",
        Tags = new[] { "Seed" }
    )]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InsertAddresses(CancellationToken cancellationToken, int num = 100)
    {
        if (num <= 0)
            return BadRequest(nameof(num) + " must be positive");

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var command = new SeedAddressCommand(num);
        await Mediator.Send(command, cancellationToken);

        stopWatch.Stop();

        return Ok(new Tuple<int, long>(num, stopWatch.ElapsedMilliseconds));
    }

    /// <summary>
    /// Seed specified number of address in database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [HttpPost("doctors", Name = "seedDoctors")]
    [SwaggerOperation(
        Summary = "Seed specified number of doctors in database",
        Description = "No or default authentication required",
        OperationId = "Seed Doctors",
        Tags = new[] { "Seed" }
    )]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDoctors(CancellationToken cancellationToken, int num = 100)
    {
        if (num <= 0)
            return BadRequest(nameof(num) + " must be positive");

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        try
        {
            var command = new SeedDoctorsCommand(num);
            await Mediator.Send(command, cancellationToken);
        }
        catch (UniqueAddressesNotAvailable ex)
        {
            return BadRequest(ex.Message);
        }

        stopWatch.Stop();

        return Ok(new Tuple<int, long>(num, stopWatch.ElapsedMilliseconds));
    }
    
}
