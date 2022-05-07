namespace CleanArchitecture.DDD.Infrastructure.Services.Base;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseEntity<T>
{
    // Create
    public Task AddEntity(T entity);

    public Task AddEntityList(IEnumerable<T> entities);

    // Read
    public IQueryable<T> GetAll();

    public Task<T?> GetById(Guid id);

    public IQueryable<T> GetByIds(IEnumerable<Guid> ids);

    // Update
    public Task UpdateEntity(Guid id, T entity, CancellationToken cancellationToken = default);

    public Task UpdateEntityList(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    // Delete
    public Task<int> DeleteEntityById(Guid id, CancellationToken cancellationToken = default);

    public Task<int> DeleteEntityListById(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    
}