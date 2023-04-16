using CleanArchitecture.DDD.API.Controllers.BaseController;
using CleanArchitecture.DDD.Application.Exceptions;
using CleanArchitecture.DDD.Core.Helpers;

namespace CleanArchitecture.DDD.API.Controllers.EntityFramework;

/// <summary>
/// 
/// </summary>
[ApiExplorerSettings(IgnoreApi = false)]
public class SeedController : BaseAPIController
{
    private const string DefaultControllerTag = "Seed";

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
        Description = DefaultDescription,
        OperationId = "Seed Addresses",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InsertAddresses(CancellationToken cancellationToken, int num = 100)
    {
        long runtime = -1;

        try
        {
            Guard.Against.NegativeOrZero(num);

            runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
            {
                var command = new SeedAddressCommand(num);
                await Mediator.Send(command, cancellationToken);

            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(new Tuple<int, long>(num, runtime));
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
        Description = DefaultDescription,
        OperationId = "Seed Doctors",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDoctors(CancellationToken cancellationToken, int num = 100)
    {
        long runtime = -1;
        
        try
        {
            Guard.Against.NegativeOrZero(num);

            runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
            {
                var command = new SeedDoctorsCommand(num);
                await Mediator.Send(command, cancellationToken);
            });
        }
        catch (UniqueAddressesNotAvailable ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(new Tuple<int, long>(num, runtime));
    }

    /// <summary>
    /// Seed specified number of doctors with address in database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [HttpPost("doctorsWithAddress", Name = "seedDoctorsWithAddress")]
    [SwaggerOperation(
        Summary = "Seed specified number of doctors with address in database",
        Description = DefaultDescription,
        OperationId = "Seed Doctors with Address",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDoctorsWithAddress(CancellationToken cancellationToken, int num = 100)
    {
        long runtime = -1;

        try
        {
            Guard.Against.NegativeOrZero(num);

            runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
            {
                var command = new SeedDoctorsWithAddressesCommand(num);
                await Mediator.Send(command, cancellationToken);


            });
        }
        catch (UniqueAddressesNotAvailable ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(new Tuple<int, long>(num, runtime));
    }

    /// <summary>
    /// Seed specified number of patients with master data
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [HttpPost("patientsWithMasterData", Name = "seedPatientsWithMasterData")]
    [SwaggerOperation(
        Summary = "Seed specified number of patient with MasterData in database",
        Description = DefaultDescription,
        OperationId = "Seed Patient",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePatientWithMasterData(CancellationToken cancellationToken, int num = 100)
    {
        Guard.Against.NegativeOrZero(num);

        var runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
        {
            var command = new SeedPatientsWithMasterDataCommand(num);
            await Mediator.Send(command, cancellationToken);

        });

        return Ok(new Tuple<int, long>(num, runtime));
    }

}
