using System.Collections.Immutable;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using DatabaseContext = Microsoft.EntityFrameworkCore.DbContext;
using CleanArchitecture.DDD.Core.ExtensionMethods;
using CleanArchitecture.DDD.Infrastructure.Persistence.Extensions;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;

public class DomainDbContext : DatabaseContext
{
    public DomainDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    #region -- Entities --

    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {      
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.ConfigureSoftDelete()
            .ConfigureGlobalFilters();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditingData();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        SetAuditingData();
        return base.SaveChanges();
    }

    /// <summary>
    /// Warning: This can degrade the performance
    /// </summary>
    private void SetAuditingData()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is {Entity: BaseEntity, State: EntityState.Added or EntityState.Modified});

        var now = DateTime.Now;

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
                ((BaseEntity)entityEntry.Entity).CreatedOn = now;
            else
                ((BaseEntity)entityEntry.Entity).UpdatedOn = now;
        }
                       
    }
}