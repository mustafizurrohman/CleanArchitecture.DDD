﻿using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.API.Controllers.EntityFramework;

public class EFDemoController(IAppServices appServices) : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "EF-Demo";

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
        Tags = new[] { DefaultControllerTag }
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
        Tags = new[] { DefaultControllerTag }
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
        Tags = new[] { DefaultControllerTag }
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
    /// TODO: Implement using MediatR
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("grouping", Name = "DemoGrouping")]
    [SwaggerOperation(
        Summary = "Grouping using Entity Framework",
        Description = DefaultDescription,
        OperationId = "EntityFramework Grouping Demo",
        Tags = new[] { "EF-Demo" }
    )]
    public async Task<IActionResult> GroupDoctorsByCity(CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var result0 = DbContext.Doctors
            // EF Core will take care of it
            // .Include(doc => doc.Address)
            .Select(doc => new
            {
                doc.FullName,
                doc.Specialization,
                doc.Address.City
            })
            .AsNoTracking()
            .OrderBy(doc => doc.City)
            .GroupBy(doc => doc.City)
            .Select(grp => new
            {
                City = grp.Key,
                Doctors = grp.Select(doc => new
                {
                    doc.FullName,
                    Specialization = doc.Specialization.ToString()
                }),
                Count = grp.Count()
            })
            .OrderByDescending(doc => doc.Count)
            .AsAsyncEnumerable();

        // Work with streaming results using EF Core!
        var result = await result0
            .Select(groupedDoctors => new
            {
                groupedDoctors.City,
                groupedDoctors.Count,
                DoctorsBySpecialization = groupedDoctors.Doctors
                    .OrderBy(doc => doc.Specialization)
                    .ThenBy(doc => doc.FullName)
                    .GroupBy(doc => doc.Specialization)
                    .Select(docSp => new
                    {
                        Specialization = docSp.Key.ToSpecialization().ToStringCached(),
                        Doctors = docSp.Select(dsp => dsp.FullName),
                        Count = docSp.Count()
                    })
            })
            .ToListAsync(cancellationToken);

        stopwatch.Stop();
        Log.Information("Query took {executionTime} ms", stopwatch.ElapsedMilliseconds);

        return Ok(result);
    }

    #pragma warning disable CS1998 
    // Async method lacks 'await' operators and will run synchronously
    /// <summary>
    /// TODO: Implement using MediatR
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("streaming", Name = "DemoStreaming")]
    [SwaggerOperation(
        Summary = "Streaming using Entity Framework",
        Description = DefaultDescription,
        OperationId = "EntityFramework streaming Demo",
        Tags = new[] { DefaultControllerTag }
    )]
    public async Task<IActionResult> GetDoctorsStreaming(CancellationToken cancellationToken)
    {
        var doctors = DbContext.Doctors
            .AsNoTracking()
            .Select(doc => doc.Name.Firstname + " " + doc.Name.Lastname)
            .Take(100)
            .ToAsyncEnumerable();

        return Ok(doctors);
    }
}