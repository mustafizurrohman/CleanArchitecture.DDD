namespace CleanArchitecture.DDD.API.Controllers;

/// <summary>
/// 
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
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
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [HttpPost("doctors")]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDoctors(CancellationToken cancellationToken, int num = 100)
    {
        if (num <= 0)
            return BadRequest("Number must be 0 or positive.");

        var faker = new Faker();

        var existingDoctorAddresses = await DbContext.Doctors
            .Select(doc => doc.AddressId)
            .ToListAsync(cancellationToken);

        var addressIds = await DbContext.Addresses
            .Select(add => add.AddressID)
            .ToListAsync(cancellationToken);

        var availableAddressIds = addressIds.Except(existingDoctorAddresses).ToList();

        if (availableAddressIds.Count < num)
            return BadRequest("Sufficient unique addresses not available");

        var names = Enumerable.Range(0, num)
            .Select(_ => Name.Create(faker.Name.FirstName(), faker.Name.LastName()))
            .ToArray();

        var doctors = Enumerable.Range(0, num)
            .Select(_ =>
            {
                var randomName = faker.Random.ArrayElement(names);
                var randomAddressGuid = faker.Random.ArrayElement(addressIds.ToArray());

                addressIds.Remove(randomAddressGuid);

                return Doctor.Create(randomName, randomAddressGuid);
            })
            .ToList();

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        await DbContext.AddRangeAsync(doctors, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        stopWatch.Stop();

        return Ok(new Tuple<int, long>(num, stopWatch.ElapsedMilliseconds));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    [HttpPost("address")]
    [ProducesResponseType(typeof(Tuple<int, long>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InsertAddresses(CancellationToken cancellationToken, int num = 100)
    {
        if (num <= 0)
            return BadRequest("Number must be 0 or positive.");

        var faker = new Faker("de");

        var fakeCountries = new List<string>()
        {
            "Deutschland",
            "Osterreich",
            "Schweiz"
        };

        var addresses = Enumerable.Range(0, num)
            .Select(_ => Address.Create(faker.Address.StreetName(), faker.Address.ZipCode(), faker.Address.City(), faker.Random.ArrayElement(fakeCountries.ToArray())))
            .ToList();
        
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        await DbContext.AddRangeAsync(addresses, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        stopWatch.Stop();

        return Ok(new Tuple<int, long>(num, stopWatch.ElapsedMilliseconds));
    }
}
