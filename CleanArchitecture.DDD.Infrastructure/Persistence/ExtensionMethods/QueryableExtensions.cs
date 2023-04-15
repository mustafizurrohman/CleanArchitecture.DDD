using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.ExtensionMethods;

public static class QueryableExtensions
{
    public static async Task<int> SoftDeleteBulkAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        return await queryable
            .ExecuteUpdateAsync(doc =>
                    doc.SetProperty(prop => prop.SoftDeleted, prop => true),
                cancellationToken: cancellationToken);
    }
}

