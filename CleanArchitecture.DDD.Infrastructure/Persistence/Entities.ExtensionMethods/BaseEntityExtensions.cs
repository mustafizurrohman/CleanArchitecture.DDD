namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;

public static class BaseEntityExtensions
{
    public static BaseEntity SoftDelete(this BaseEntity baseEntity)
    {
        baseEntity.SoftDeleted = true;
        return baseEntity;
    }

    public static IEnumerable<T> SoftDelete<T>(this IEnumerable<T> baseEntities)
            where T : BaseEntity
    {
        return baseEntities
            .Select(be => {
                be.SoftDeleted = true;
                return be;
            });
    }
}
