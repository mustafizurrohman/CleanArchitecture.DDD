using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DatabaseContext = Microsoft.EntityFrameworkCore.DbContext;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;

public partial class DomainDbContext : DatabaseContext
{
    public DomainDbContext()
    {
    }

    public DomainDbContext(DbContextOptions<DomainDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doctor> Doctors { get; set; }
}