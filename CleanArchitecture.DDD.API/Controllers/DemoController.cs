using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;
using System.Security.Cryptography;

namespace CleanArchitecture.DDD.API.Controllers;

public class DemoController : BaseAPIController
{
    private int RandomDelay => RandomNumberGenerator.GetInt32(750, 1000);

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
    [HttpPost("loggingCorrelation", Name = "loggingCorrelation")]
    [SwaggerOperation(
        Summary = "Demo of exception logging and tracibility and support code",
        Description = DefaultDescription,
        OperationId = "Log Exception",
        Tags = new[] { "Demo" }
    )]
    public IActionResult TestExceptionLogging()
    {
        var faker = new Faker();

        Thread.Sleep(RandomDelay);
        Log.Information("Testing if logs are corelated. {param} is a random parameter", faker.Lorem.Word());
        
        Thread.Sleep(RandomDelay);
        Log.Verbose("A verbose log message");
        
        Thread.Sleep(RandomDelay);
        Log.Debug("Here is a debug message with param {message}", "debug message param");
        
        Thread.Sleep(RandomDelay);
        Log.Fatal("This should actually never happen {param}", faker.Lorem.Sentence());
        
        Thread.Sleep(RandomDelay);
        Log.Error("Logging before throwing an exception .... ");
        
        Thread.Sleep(RandomDelay);
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
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("softdelete", Name = "softdelete")]
    [SwaggerOperation(
        Summary = "Demo of soft delete extension method",
        Description = DefaultDescription,
        OperationId = "Demo soft delete Extension Method",
        Tags = new[] { "Demo" }
    )]
    public async Task<IActionResult> DemoSoftDelete(Guid doctorGuid, CancellationToken cancellationToken)
    {
        var doctor = await DbContext.Doctors
            .FindAsync(doctorGuid, cancellationToken);

        if (doctor is null)
            return BadRequest("Invalid doctor Guid.");

        doctor.SoftDelete();
        await DbContext.SaveChangesAsync(cancellationToken);

        return Ok(doctor);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("softdelete/collection", Name = "softDeleteCollection")]
    [SwaggerOperation(
        Summary = "Demo of soft delete extension method on IEnumerable",
        Description = DefaultDescription,
        OperationId = "Demo soft delete Extension Method on IEnumerable",
        Tags = new[] { "Demo" }
    )]
    public async Task<IActionResult> DemoSoftDeleteCollection(CancellationToken cancellationToken)
    {
        var doctors = await DbContext.Doctors
            // This query will select only not deleted entries due to Global filter of Doctors
            .OrderBy(doc => Guid.NewGuid())
            .Take(20)
            .ToListAsync(cancellationToken);
           
        doctors = doctors.SoftDelete().ToList();
        await DbContext.SaveChangesAsync(cancellationToken);

        return Ok(doctors);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("softdelete/collection/undo", Name = "UndoSoftDeleteCollection")]
    [SwaggerOperation(
        Summary = "Demo of undo soft delete extension method on IEnumerable",
        Description = DefaultDescription,
        OperationId = "Demo undo soft delete Extension Method on IEnumerable",
        Tags = new[] { "Demo" }
    )]
    public async Task<IActionResult> DemoUndoSoftDeleteCollection(CancellationToken cancellationToken)
    {
        var doctors = await DbContext.Doctors
            // This query will select only not deleted entries due to Global filter of Doctors
            // Unless 'IgnoreQueryFilters' is specified
            .IgnoreQueryFilters()
            .Where(doc => doc.SoftDeleted)
            .OrderBy(doc => Guid.NewGuid())
            .Take(20)
            .ToListAsync(cancellationToken);

        doctors = doctors.UndoSoftDelete().ToList();
        await DbContext.SaveChangesAsync(cancellationToken);

        return Ok(doctors);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("weischer", Name = "DemoWeischer")]
    [SwaggerOperation(
        Summary = "Placeholder for demo for Weischer Colleagues",
        Description = DefaultDescription,
        OperationId = "Demo for Weischer",
        Tags = new[] { "Demo" }
    )]
    public async Task<IActionResult> WeischerDemo(CancellationToken cancellationToken)
    {
        Thread.Sleep(10000);

        
        var doctors = await DbContext.Doctors
            // This query will select only not deleted entries due to Global filter of Doctors
            // Unless 'IgnoreQueryFilters' is specified
            .IgnoreQueryFilters()
            .Where(doc => doc.SoftDeleted)
            .OrderBy(doc => Guid.NewGuid())
            .Take(20)
            .ToListAsync(cancellationToken);
        
        return Ok(doctors);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("log", Name = "DemoLogging")]
    [SwaggerOperation(
        Summary = "Demo for Generation of logs for Seq Visualization",
        Description = DefaultDescription,
        OperationId = "Demo of log generation",
        Tags = new[] { "Demo" }
    )]
    public async Task<IActionResult> LogGenerationDemo(CancellationToken cancellationToken, int iterations = 10, bool withDelay = false)
    {
        var generateLogsCommand = new GenerateLogsCommand(iterations, withDelay);
        await Mediator.Send(generateLogsCommand, cancellationToken);

        return Ok();
    }

}