using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;

namespace CleanArchitecture.DDD.API.Controllers;

public class EntityFrameworkDemoController : BaseAPIController
{
    public EntityFrameworkDemoController(IAppServices appServices)
        : base(appServices)
    {
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
        Tags = new[] { "EntityFramework" }
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
        Tags = new[] { "EntityFramework" }
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
        Tags = new[] { "EntityFramework" }
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
    [HttpGet("grouping", Name = "DemoWeischer")]
    [SwaggerOperation(
        Summary = "Grouping using Entity Framework",
        Description = DefaultDescription,
        OperationId = "EntityFramework Grouping Demo",
        Tags = new[] { "EntityFramework" }
    )]
    public async Task<IActionResult> GroupDoctorsByCity(CancellationToken cancellationToken)
    {
        var result = (await DbContext.Doctors
            .Include(doc => doc.Address)
            .OrderBy(doc => doc.Address.City)
            .GroupBy(doc => doc.Address.City)
            .Select(grp => new
            {
                City = grp.Key,
                Doctors = grp.Select(x => x.FullName),
                Count = grp.Count()
            })
            .OrderByDescending(doc => doc.Count)
            .ToListAsync(cancellationToken))
            .Select(groupedDoctors => groupedDoctors with
            {
                Doctors = groupedDoctors.Doctors.OrderBy(docName => docName)
            });

        return Ok(result);
    }
}