﻿using CleanArchitecture.DDD.Core.ExtensionMethods;
using CleanArchitecture.DDD.Core.Helpers;
using CleanArchitecture.DDD.Infrastructure.Exceptions;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using DatabaseContext = Microsoft.EntityFrameworkCore.DbContext;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;

public class DomainDbContext : DatabaseContext
{
    private readonly string _connectionString;
    private readonly bool _useLogger;
    
    private DomainDbContext()
    {
    }

    public DomainDbContext(string connectionString, bool useLogger)
    {
        _connectionString = connectionString;

        if (!_connectionString.IsValidDbConnectionString())
        {
            const string message = "Invalid database connection string or database is not reachable ... ";
            
            Log.Fatal(message);
            throw new DatabaseNotReachableException();
        } 

        _useLogger = useLogger;
    }

    #region -- Entities --

    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }

    #endregion
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);

        if (_useLogger)
        {
            var consoleLoggerFactory = LoggerFactory.Create(loggerBuilder =>
            {
                loggerBuilder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddConsole();
            });
            
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(consoleLoggerFactory);
        }
        else
        {
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(builder =>
                {
                    builder.AddFilter((_, _) => false);
                }));
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {      
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region -- Soft Deletion Configuration --

        var allEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .ToList();

        // Set default value of SoftDeletedProperty in all entities
        var allSoftDeletedProperties = allEntityTypes
            .SelectMany(type => type.GetProperties())
            .Where(p => p.Name == nameof(BaseEntity.SoftDeleted))
            .ToList();

        foreach (var prop in allSoftDeletedProperties)
        {
            prop.SetDefaultValue(false);
        }

        #endregion

        #region -- Global Query Filter Configuration --

        // TODO: Can this be done using reflection?
        modelBuilder.Entity<Doctor>()
            .HasQueryFilter(doc => !doc.SoftDeleted);

        modelBuilder.Entity<Address>()
            .HasQueryFilter(addr => !addr.SoftDeleted);

        #endregion
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
        BenchmarkHelper.Benchmark(() =>
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity
                            && e.State is EntityState.Added or EntityState.Modified);

            var now = DateTime.Now;

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                    ((BaseEntity)entityEntry.Entity).CreatedOn = now;
                else
                    ((BaseEntity)entityEntry.Entity).UpdatedOn = now;
            }
        });                
    }
}