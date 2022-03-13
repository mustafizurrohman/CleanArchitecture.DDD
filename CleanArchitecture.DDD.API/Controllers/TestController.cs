using CleanArchitecture.DDD.Domain.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.DDD.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IValidator<Name> _nameValidator;

    public TestController(IValidator<Name> nameValidator)
    {
        _nameValidator = nameValidator;
    }

    [HttpGet]
    public IActionResult CreateName([FromQuery] string firstname, [FromQuery] string lastname)
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
    public IActionResult CreateName([FromBody] Name name)
    {
        return Ok(name.ToString());
    }
}