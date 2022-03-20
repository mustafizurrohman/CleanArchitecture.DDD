using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.DDD.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IValidator<Name> _nameValidator;
    private readonly DomainDbContext _dbContext;

    public TestController(IValidator<Name> nameValidator, DomainDbContext dbContext)
    {
        _nameValidator = nameValidator;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult CreateName([FromQuery] string? firstname, [FromQuery] string? lastname)
    {
        var name = new Name(firstname, lastname);

        var validationResult = _nameValidator.Validate(name);

        if (validationResult.IsValid)
        {
            return Ok();
        }

        var errors = validationResult.Errors
            .Select(x => x.ErrorMessage)
            .ToList();

        return BadRequest(errors);
    }

    [HttpPost]
    public async  Task<IActionResult> CreateName([FromBody] Name name)
    {
        var doctor = new Doctor()
        {
            Name = name
        };

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
    public async Task<IActionResult> Search([FromQuery] string? firstname, [FromQuery] string? middlename,
        [FromQuery] string? lastname)
    {
        var name = new Name(firstname, middlename, lastname);

        var doctors = await _dbContext.Doctors.FirstOrDefaultAsync(doc => doc.Name == name);
        
        return Ok(doctors);
    }
}