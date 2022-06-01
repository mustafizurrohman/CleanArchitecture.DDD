namespace CleanArchitecture.DDD.API.Controllers;

[ApiExplorerSettings(IgnoreApi = false)]
public class DemoController : BaseAPIController
{
    private readonly IValidator<Name> _nameValidator;

    public DemoController(IValidator<Name> nameValidator, IAppServices appServices)
        : base(appServices)
    {
        _nameValidator = nameValidator;
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

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("exception", Name = "loggingException")]
    [SwaggerOperation(
        Summary = "Demo of exception logging and support code",
        Description = "No or default authentication required",
        OperationId = "Log Exception",
        Tags = new[] { "Demo" }
    )]
    public IActionResult TestExceptionLogging()
    {
        throw new NotImplementedException();
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("inputValidation")]
    [SwaggerOperation(
        Summary = "Demo of input validation using FluentValidation",
        Description = "No or default authentication required",
        OperationId = "Input Validation",
        Tags = new[] { "Demo" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateName([FromBody] Name name)
    {
        return Ok();
    }

}