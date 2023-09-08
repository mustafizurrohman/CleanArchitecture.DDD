namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;

public abstract class BaseEntity
{
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public bool SoftDeleted { get; set; }

    // TODO: Show Demo with a migration!
    // public DateTime? SoftDeletedOn { [UsedImplicitly] get; set; }
}