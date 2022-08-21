using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.API.Controllers;

public class DemoController : BaseAPIController
{
    

    public DemoController(IAppServices appServices)
        : base(appServices)
    {
    }

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
        Tags = new[] { "Demo" }
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
    [HttpGet("extension", Name = "extension")]
    [SwaggerOperation(
        Summary = "Test of random specialization as text",
        Description = DefaultDescription,
        OperationId = "Demo Specialization Enum",
        Tags = new[] { "Demo" }
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



}