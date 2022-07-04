using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;
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

        var name1 = createdName1.Value;
        var name2 = createdName2.Value;

        return Ok(name1 == name2);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("exception", Name = "loggingException")]
    [SwaggerOperation(
        Summary = "Demo of exception logging and support code",
        Description = DefaultDescription,
        OperationId = "Log Exception",
        Tags = new[] { "Demo" }
    )]
    public IActionResult TestExceptionLogging()
    {
        throw new NotImplementedException();
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("softdelete", Name = "softdelete")]
    [SwaggerOperation(
        Summary = "Demo of softdelete extension method",
        Description = DefaultDescription,
        OperationId = "Demo softdelete Extension Method",
        Tags = new[] { "Demo" }
    )]
    public async Task<IActionResult> DemoSoftDelete(Guid doctorGuid, CancellationToken cancellationToken)
    {
        var doctor = await DbContext.Doctors
            .FindAsync(doctorGuid);

        if (doctor is null)
            return BadRequest("Invalid doctor Guid.");

        doctor.SoftDelete();
        await DbContext.SaveChangesAsync(cancellationToken);

        return Ok(doctor);
    }

}