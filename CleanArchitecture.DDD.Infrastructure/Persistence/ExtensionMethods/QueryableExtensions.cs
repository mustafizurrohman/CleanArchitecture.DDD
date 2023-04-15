using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.ExtensionMethods;

public static class QueryableExtensions
{
    public static async Task<int> SoftDeleteBulkAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        var now = DateTime.Now;

        return await queryable
            .ExecuteUpdateAsync(doc =>
                    doc.SetProperty(prop => prop.SoftDeleted, prop => true)
                        .SetProperty(prop => prop.UpdatedOn, prop => now), cancellationToken: cancellationToken);
    }

    public static async Task<int> UndoSoftDeleteBulkAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        var now = DateTime.Now;

        return await queryable
            .ExecuteUpdateAsync(doc =>
                    doc.SetProperty(prop => prop.SoftDeleted, prop => false)
                       .SetProperty(prop => prop.UpdatedOn, prop => now), cancellationToken: cancellationToken);
    }
}

