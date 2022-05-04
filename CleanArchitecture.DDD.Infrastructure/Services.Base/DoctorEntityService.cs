using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
using Z.EntityFramework.Plus;

namespace CleanArchitecture.DDD.Infrastructure.Services.Base
{
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
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddEntityList(IEnumerable<Doctor> entities)
        {
            await _dbContext.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Doctor> GetAll()
        {
            return _dbContext.Doctors.AsQueryable();
        }

        public async Task<Doctor?> GetById(Guid id)
        {
            return await _dbContext.Doctors.FindAsync(id);
        }

        public IQueryable<Doctor> GetByIds(IEnumerable<Guid> ids)
        {
            return _dbContext.Doctors
                .Where(doc => ids.Contains(doc.DoctorID));

        }

        public async Task UpdateEntity(Guid id, Doctor entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEntityList(IEnumerable<Doctor> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
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
}
