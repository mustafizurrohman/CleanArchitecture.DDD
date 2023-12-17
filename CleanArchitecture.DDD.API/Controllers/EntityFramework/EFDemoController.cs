using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.API.Controllers.EntityFramework;

public class EFDemoController(IAppServices appServices) 
    : BaseAPIController(appServices)
{
    private const string DefaultControllerTag = "EF-Demo";
    
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
        Tags = ["EF-Demo"]
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

    /// <summary>
    /// Must ba a Async function but without await
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpGet("streaming", Name = "DemoStreaming")]
    [SwaggerOperation(
        Summary = "Streaming using Entity Framework",
        Description = DefaultDescription,
        OperationId = "EntityFramework streaming Demo",
        Tags = [DefaultControllerTag]
    )]
    public async Task<IActionResult> GetDoctorsStreaming(CancellationToken cancellationToken)
    {
        var doctors = DbContext.Doctors
            .OrderBy(doc => doc.Name.Lastname)
            .ThenBy(doc => doc.Name.Firstname)
            .AsNoTracking()
            .Select(doc => doc.Name.Firstname + " " + doc.Name.Lastname)
            .Take(100)
            .ToAsyncEnumerable();

        // Also works!
        // return Ok(doctors); 
        return Ok(await doctors.ToListAsync(cancellationToken));
    }
}