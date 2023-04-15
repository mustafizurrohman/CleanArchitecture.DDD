﻿using System.Runtime.CompilerServices;
using CleanArchitecture.DDD.Core.Helpers;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;
using CleanArchitecture.DDD.Infrastructure.Persistence.ExtensionMethods;

namespace CleanArchitecture.DDD.API.Controllers.EntityFramework;

public class EFBulkExtensionsDemoController : BaseAPIController
{
    public EFBulkExtensionsDemoController(IAppServices appServices)
        : base(appServices)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("softdelete/collection/bulk", Name = "softDeleteCollectionBulk")]
    [SwaggerOperation(
        Summary = "Demo of soft delete extension method on IEnumerable using Bulk Extension methods",
        Description = DefaultDescription,
        OperationId = "Demo soft delete Extension Method on IEnumerable using Bulk Extension methods",
        Tags = new[] { "EF-BulkExtensions" }
    )]
    public async Task<IActionResult> DemoSoftDeleteBulkCollection(CancellationToken cancellationToken)
    {
        var affectedRows = -1;

        await Helper.BenchmarkAsync(async () =>
        {
            affectedRows = await DbContext.Addresses
                .OrderBy(_ => Guid.NewGuid())
                .Take(20)
                .SoftDeleteBulkAsync(cancellationToken);
        });

        return Ok(affectedRows);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("softdelete/collection/bulk/undo", Name = "softDeleteCollectionUndoBulk")]
    [SwaggerOperation(
        Summary = "Demo of undo soft delete extension method on IEnumerable using Bulk Extension methods",
        Description = DefaultDescription,
        OperationId = "Demo undo soft delete Extension Method on IEnumerable using Bulk Extension methods",
        Tags = new[] { "EF-BulkExtensions" }
    )]
    public async Task<IActionResult> DemoUndoSoftDeleteUndoBulkCollection(CancellationToken cancellationToken)
    {
        var affectedRows = -1;

        await Helper.BenchmarkAsync(async () =>
        {
            affectedRows = await DbContext.Addresses
                .IgnoreQueryFilters()
                .Where(add => add.SoftDeleted)
                .OrderBy(_ => Guid.NewGuid())
                .Take(20)
                .UndoSoftDeleteBulkAsync(cancellationToken);
        });

        return Ok(affectedRows);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = false)]
    [HttpPost("delete/collection/bulk", Name = "DeleteCollectionBulk")]
    [SwaggerOperation(
        Summary = "Demo of bulk delete",
        Description = DefaultDescription,
        OperationId = "Demo of bulk delete",
        Tags = new[] { "EF-BulkExtensions" }
    )]
    public async Task<IActionResult> DemoBulkDelete(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var affectedRows = -1;

        await Helper.BenchmarkAsync(async () =>
        {
            affectedRows = await DbContext.Addresses
                .IgnoreQueryFilters()
                .Where(adr => ids.Contains(adr.AddressID))
                .ExecuteDeleteAsync(cancellationToken);
        });

        return Ok(affectedRows);
    }
}

