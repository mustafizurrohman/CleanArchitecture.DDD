using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.API.Controllers;

public class DemoController(IAppServices appServices) 
    : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "Demo";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("ValueObject/equality", Name = "valueObjectEquality")]
    [SwaggerOperation(
        Summary = "Demo of equality of Value Object",
        Description = DefaultDescription,
        OperationId = "Check Value Object for equality",
        Tags = new[] { DefaultControllerTag }
    )]
    public IActionResult TestNameValueObjectForEquality(string name)
    {
        var createdName1 = NameValueObject.Create(name);
        var createdName2 = NameValueObject.Create(name);

        if (createdName1.IsFailure || createdName2.IsFailure)
            return BadRequest(createdName2.Error.Message);
        
        return Ok(createdName1.Value == createdName2.Value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("extension/enum", Name = "extensionEnum")]
    [SwaggerOperation(
        Summary = "Test of random specialization as text",
        Description = DefaultDescription,
        OperationId = "Demo Specialization Enum",
        Tags = new[] { DefaultControllerTag }
    )]
    public IActionResult DemoExtensionMethod()
    {
        var randomSpecializations = Enumerable.Range(0, 1000)
            .Select(_ => SpecializationEnumExtensions.GetRandomSpecialization())
            .Select(sp => new
            {
                Specialization = sp,
                SpecializationAsString = sp.ToStringCached()
            })
            .GroupBy(sp => sp.Specialization)
            .Select(sp => new
            {
                SpAsString = sp.Key.ToStringCached(),
                Count = sp.Count()
            })
            .OrderByDescending(sp => sp.Count)
            .ToList();

        return Ok(randomSpecializations);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("extension/parse", Name = "extensionParse")]
    [SwaggerOperation(
        Summary = "Test of extension method for parsing",
        Description = DefaultDescription,
        OperationId = "Demo Parsing",
        Tags = new[] { DefaultControllerTag }
    )]
    public IActionResult DemoExtensionMethodParse()
    {
        var randomNumber1 = ((new Faker()).Random.Number(50, 100)).ToString();
        var randomNumber2 = ((new Faker()).Random.Number(100, 200)).ToString();

        var num1Parsed = randomNumber1.Parse<int>();
        var num2Parsed = randomNumber2.Parse<double>(); 

        return Ok((num1Parsed, num2Parsed));
    }

}
