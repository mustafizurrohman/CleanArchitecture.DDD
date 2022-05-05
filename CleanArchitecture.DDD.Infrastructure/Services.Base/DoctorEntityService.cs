using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace CleanArchitecture.DDD.Infrastructure.Services.Base;

public class DoctorEntityService : IBaseEntity<Doctor>
{
    private readonly DomainDbContext _dbContext;

    public DoctorEntityService(DomainDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddEntity(Doctor entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public async Task AddEntityList(IEnumerable<Doctor> entities)
    {
        await _dbContext.AddRangeAsync(entities);
    }

    public IQueryable<Doctor> GetAll()
    {
        return _dbContext.Doctors
            .AsNoTracking()
            .AsQueryable();
    }

    public async Task<Doctor?> GetById(Guid id)
    {
        return await _dbContext.Doctors.FindAsync(id);
    }

    public IQueryable<Doctor> GetByIds(IEnumerable<Guid> ids)
    {
        return _dbContext.Doctors
            .AsNoTracking()
            .Where(doc => ids.Contains(doc.DoctorID));

    }

    // TODO: Test and explore other possibilities
    public async Task UpdateEntity(Guid id, Doctor entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Doctors
            .Where(doc => doc.DoctorID == id)
            .UpdateAsync(_ => Doctor.Create(entity.Name, entity.Address), cancellationToken);
    }

    // TODO: Test and explore other possibilities
    public Task UpdateEntityList(IEnumerable<Doctor> entities)
    {
        // _dbContext.UpdateRange(entities);
        throw new NotImplementedException();
    }

    public async Task DeleteEntityById(Guid id, CancellationToken cancellationToken = default)
    {
        await _dbContext.Doctors
            .Where(doc => doc.DoctorID == id)
            .DeleteAsync(cancellationToken);
    }

    public async Task DeleteEntityListById(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        await _dbContext.Doctors
            .Where(doc => ids.Contains(doc.DoctorID))
            .DeleteAsync(cancellationToken);
    }
}