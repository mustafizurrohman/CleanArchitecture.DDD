using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.ExtensionMethods;

public static class QueryableExtensions
{
    public static async Task<int> SoftDeleteBulkAsync<T>(this IQueryable<T> queryable, 
        CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return await queryable.ExecuteSoftDeleteOperation(softDeleteValue: true, cancellationToken);
    }

    public static async Task<int> UndoSoftDeleteBulkAsync<T>(this IQueryable<T> queryable, 
        CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return await queryable.ExecuteSoftDeleteOperation(softDeleteValue: false, cancellationToken);
    }

    private static async Task<int> ExecuteSoftDeleteOperation<T>(this IQueryable<T> queryable,
        bool softDeleteValue, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        var now = DateTime.UtcNow;

        return await queryable
            .ExecuteUpdateAsync(doc =>
                    doc.SetProperty(prop => prop.SoftDeleted, prop => softDeleteValue)
                       .SetProperty(prop => prop.UpdatedOn, prop => now),
                cancellationToken: cancellationToken);
    }
}
