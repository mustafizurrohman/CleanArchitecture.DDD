namespace CleanArchitecture.DDD.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class TestController : BaseAPIController
{
    private readonly IValidator<Name> _nameValidator;
    private readonly ILogger<TestController> _logger;
    private readonly IEDCMSyncService _iedcmSyncService;

    public TestController(IValidator<Name> nameValidator, DomainDbContext dbContext, IMapper autoMapper, ILogger<TestController> logger, IEDCMSyncService iedcmSyncService)
        : base(dbContext, autoMapper)
    {
        _nameValidator = nameValidator;
        _logger = logger;
        _iedcmSyncService = iedcmSyncService;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("doctors", Name = "GetAllDoctors")]
    [ProducesResponseType(typeof(IEnumerable<Doctor>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await DbContext.Doctors.ToListAsync();
        return Ok(doctors);
    }



    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("TestNameValidation")]
    public IActionResult CreateName([FromQuery] string? firstname, [FromQuery] string? lastname)
    {
        var name = Name.Create(firstname ?? string.Empty, lastname ?? string.Empty, false);

        var validationResult = _nameValidator.Validate(name);

        if (validationResult.IsValid)
            return Ok();

        var errors = validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToList();

        return BadRequest(errors);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("demo")]
    public async  Task<IActionResult> CreateName([FromBody] Name name)
    {
        var doctor = Doctor.Create(name.Firstname, name.Middlename, name.Lastname);

        DbContext.Doctors.Add(doctor);
        await DbContext.SaveChangesAsync();

        var allDoctors = await DbContext.Doctors.AsNoTracking().ToListAsync();

        return Ok(allDoctors);
    }

    // Test only
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("search")]
    public async Task<IActionResult> Search(CancellationToken cancellationToken, [FromQuery] string? firstname, [FromQuery] string? lastname, [FromQuery] bool and = false)
    {
        var name = Name.Create(firstname ?? string.Empty, lastname ?? string.Empty);

        // TODO: Must be possible to search only by FirstOrLastname and/or Lastname
        var validationResult = await _nameValidator.ValidateAsync(name, cancellationToken);

        if (!validationResult.IsValid && and)
            return Ok();
        
        var doctors = await DbContext.Doctors.AsNoTracking()
            .SearchByName(name, and)
            .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
         
        return Ok(doctors);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("test")]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        var results = await DbContext.Doctors.AsNoTracking()
            .ProjectTo<DoctorCityDTO>(AutoMapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Ok(results);
    }



    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("compare/names")]
    public IActionResult CompareNames(string firstname, string middlename, string lastname, CancellationToken cancellationToken)
    {
        var name1 = new Name(firstname, middlename, lastname);
        var name2 = new Name(firstname, middlename, lastname);

        return Ok(name1 == name2);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("testValueObject")]
    public IActionResult TestNameValueObject(string name)
    {
        var createdName = NameValueObject.Create(name);

        if (createdName.Error is not null)
            return BadRequest(createdName.Error.Message);

        var x = createdName.Value;

        return Ok();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("exceptionLogging")]
    public IActionResult TestExceptionLogging()
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Log.Error(e, "Unhandled exception");
        }

        return Ok();
    }

}