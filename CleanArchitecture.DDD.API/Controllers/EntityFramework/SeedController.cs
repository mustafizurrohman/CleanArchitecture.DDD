using CleanArchitecture.DDD.Application.MediatR.Commands.SeedAddress;
using CleanArchitecture.DDD.Application.MediatR.Commands.SeedDoctors;
using CleanArchitecture.DDD.Application.MediatR.Commands.SeedDoctorsWithAdresses;
using CleanArchitecture.DDD.Application.MediatR.Commands.SeedPatientsWithMasterData;

namespace CleanArchitecture.DDD.API.Controllers.EntityFramework;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// 
/// </remarks>
/// <param name="appServices"></param>
[ApiExplorerSettings(IgnoreApi = false)]
public class SeedController(IAppServices appServices) 
    : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "Seed";

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
        try
        {
            Guard.Against.NegativeOrZero(num);

            var runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
            {
                var command = new SeedAddressCommand(num);
                await Mediator.Send(command, cancellationToken);

            });

            return Ok(new Tuple<int, long>(num, runtime));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
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
        try
        {
            Guard.Against.NegativeOrZero(num);

            var runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
            {
                var command = new SeedDoctorsCommand(num);
                await Mediator.Send(command, cancellationToken);
            });

            return Ok(new Tuple<int, long>(num, runtime));
        }
        catch (UniqueAddressesNotAvailable ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Seed specified number of doctors with address in database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="num"></param>
    /// <param name="withDelay"></param>
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
    public async Task<IActionResult> CreateDoctorsWithAddress(CancellationToken cancellationToken, int num = 100, bool withDelay = false)
    {
        try
        {
            Guard.Against.NegativeOrZero(num);

            var runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
            {
                var command = new SeedDoctorsWithAddressesCommand(num, withDelay);
                await Mediator.Send(command, cancellationToken);
            });

            return Ok(new Tuple<int, long>(num, runtime));
        }
        catch (UniqueAddressesNotAvailable ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
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

    /// <summary>
    /// Prune the database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("prune", Name = "prune")]
    [SwaggerOperation(
        Summary = "Prune database",
        Description = DefaultDescription,
        OperationId = "Prune database",
        Tags = new[] { DefaultControllerTag }
    )]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PruneDatabase(CancellationToken cancellationToken)
    {
        var runtime = await BenchmarkHelper.BenchmarkAsync(async () =>
        {
            await DbContext.Addresses.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await DbContext.Doctors.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
            await DbContext.Patients.IgnoreQueryFilters().ExecuteDeleteAsync(cancellationToken);
        });

        return Ok(runtime);
    }
}
