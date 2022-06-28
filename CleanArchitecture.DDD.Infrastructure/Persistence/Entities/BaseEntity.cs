namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

public abstract class BaseEntity
{
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}