using JetBrains.Annotations;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

public abstract class BaseEntity
{
    public DateTime? CreatedOn { [UsedImplicitly] get; set; }
    public DateTime? UpdatedOn { [UsedImplicitly] get; set; }
    public bool SoftDeleted { [UsedImplicitly] get; set; }

    // TODO: Show Demo with a migration!
    // public DateTime? SoftDeletedOn { [UsedImplicitly] get; set; }
}