using AutoMapper;
using Bogus;

namespace CleanArchitecture.DDD.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IValidator<Name> _nameValidator;
    private readonly DomainDbContext _dbContext;
    private readonly IMapper _autoMapper;

    public TestController(IValidator<Name> nameValidator, DomainDbContext dbContext, IMapper autoMapper)
    {
        _nameValidator = nameValidator;
        _dbContext = dbContext;
        _autoMapper = autoMapper;
    }

    [HttpGet]
    public IActionResult CreateName([FromQuery] string? firstname, [FromQuery] string? lastname)
    {
        var name = new Name(firstname ?? string.Empty, lastname ?? string.Empty);

        var validationResult = _nameValidator.Validate(name);

        if (validationResult.IsValid)
            return Ok();
        

        var errors = validationResult.Errors
            .Select(x => x.ErrorMessage)
            .ToList();

        return BadRequest(errors);
    }

    [HttpPost]
    public async  Task<IActionResult> CreateName([FromBody] Name name)
    {
        var doctor = Doctor.Create(name.Firstname, name.Middlename, name.Lastname);

        _dbContext.Doctors.Add(doctor);
        await _dbContext.SaveChangesAsync();

        var allDoctors = await _dbContext.Doctors.AsNoTracking().ToListAsync();

        return Ok(allDoctors);
    }

    [HttpGet("doctors")]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _dbContext.Doctors.ToListAsync();
        return Ok(doctors);
    }
        
    // Test only
    [HttpGet("search")]
    public async Task<IActionResult> Search(CancellationToken cancellationToken, [FromQuery] string? firstname, [FromQuery] string? lastname, [FromQuery] bool and = false)
    {
        var name = new Name(firstname ?? string.Empty, lastname ?? string.Empty);

        var validationResult = await _nameValidator.ValidateAsync(name, cancellationToken);

        if (!validationResult.IsValid && and)
            return Ok();
        
        var doctors = await _dbContext.Doctors.AsNoTracking()
            .SearchByName(name, and)
            .ProjectTo<DoctorCityDTO>(_autoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
         
        return Ok(doctors);
    }

    /*
    [HttpGet("search/v2")]
    public async Task<IActionResult> SearchV2(CancellationToken cancellationToken, [FromQuery] string? firstname, [FromQuery] string? lastname, [FromQuery] bool and = false)
    {
        var name = new Name(firstname ?? string.Empty, lastname ?? string.Empty);
                
        var doctors = await _dbContext.Doctors.AsNoTrackingWithIdentityResolution()
            .Where(doc => doc.Name == Name.Copy(name))
            .ToListAsync(cancellationToken);

        return Ok(doctors);
    } */    

    [HttpGet("test")]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        var results = await _dbContext.Doctors.AsNoTracking()
            .ProjectTo<DoctorCityDTO>(_autoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Ok(results);
    }

    [HttpPost("insert/doctors")]
    public async Task<IActionResult> CreateDoctors(CancellationToken cancellationToken, int num = 100)
    {
        var faker = new Faker();

        var names = Enumerable.Range(0, num)
            .Select(_ => new Name(faker.Name.FirstName(), faker.Name.LastName()))
            .ToArray();

        var existingDoctorAddresses = await _dbContext.Doctors
            .Select(doc => doc.AddressId)
            .ToListAsync(cancellationToken);

        var addressIds = await _dbContext.Addresses
            .Select(add => add.AddressID)
            .ToListAsync(cancellationToken);

        var availableAddressIds = addressIds.Except(existingDoctorAddresses).ToList();

        if (availableAddressIds.Count < num)
            return BadRequest("Sufficient unique addresses not available");
        
        var doctors = Enumerable.Range(0, num)
            .Select(_ =>
            {
                var randomName = faker.Random.ArrayElement(names);
                var randomAddressGuid = faker.Random.ArrayElement(addressIds.ToArray());

                addressIds.Remove(randomAddressGuid);

                return Doctor.Create(randomName, randomAddressGuid);
            })
            .ToList();

        await _dbContext.Doctors.AddRangeAsync(doctors, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);    

        return Ok(await _dbContext.Doctors.CountAsync(cancellationToken));
    }

    [HttpPost("insert/address")]
    public async Task<IActionResult> InsertAddresses(CancellationToken cancellationToken, int num = 100)
    {
        var faker = new Faker();

        var addresses = Enumerable.Range(0, num)
            .Select(_ => Address.Create(faker.Address.StreetName(), faker.Address.ZipCode(), faker.Address.City(), faker.Address.Country()))
            .ToList();

        await _dbContext.Addresses.AddRangeAsync(addresses, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok();

    }

    [HttpGet("compare/names")]
    public IActionResult CompareNames(string firstname, string middlename, string lastname, CancellationToken cancellationToken)
    {
        var name1 = new Name(firstname, middlename, lastname);
        var name2 = new Name(firstname, middlename, lastname);

        return Ok(name1 == name2);
    }
       
}
