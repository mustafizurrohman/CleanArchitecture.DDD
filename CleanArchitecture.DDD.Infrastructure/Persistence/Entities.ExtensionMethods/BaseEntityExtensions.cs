using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
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
    
    public static BaseEntity UndoSoftDelete(this BaseEntity baseEntity)
    {
        baseEntity.SoftDeleted = false;
        return baseEntity;
    }

    public static IEnumerable<T> UndoSoftDelete<T>(this IEnumerable<T> baseEntities)
            where T : BaseEntity
    {
        return baseEntities
            .Select(be => {
                be.SoftDeleted = false;
                return be;
            });
    }
}
