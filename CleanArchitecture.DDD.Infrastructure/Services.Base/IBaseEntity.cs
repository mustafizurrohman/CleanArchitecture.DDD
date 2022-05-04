namespace CleanArchitecture.DDD.Infrastructure.Services.Base
{
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
        public Task UpdateEntity(Guid id, T entity);

        public Task UpdateEntityList(IEnumerable<T> entities);

        // Delete
        public Task DeleteEntityById(Guid id, CancellationToken cancellationToken = default);

        public Task DeleteEntityListById(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    
    }
}