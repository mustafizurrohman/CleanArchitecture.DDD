using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
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
            .UpdateAsync(_ => Doctor.Copy(entity), cancellationToken);
    }

    // TODO: Test and explore other possibilities
    public async Task UpdateEntityList(IEnumerable<Doctor> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateEntity(entity.DoctorID, entity, cancellationToken);
        }
    }

    public async Task<int> DeleteEntityById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Doctors
            .Where(doc => doc.DoctorID == id)
            .DeleteAsync(cancellationToken);
    }

    public async Task<int> DeleteEntityListById(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Doctors
            .Where(doc => ids.Contains(doc.DoctorID))
            .DeleteAsync(cancellationToken);
    }
}