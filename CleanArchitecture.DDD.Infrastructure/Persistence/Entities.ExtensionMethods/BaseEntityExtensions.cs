namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.ExtensionMethods;

public static class BaseEntityExtensions
{
    public static BaseEntity SoftDelete(this BaseEntity baseEntity)
    {
        baseEntity.SoftDeleted = true;
        return baseEntity;
    }
}
