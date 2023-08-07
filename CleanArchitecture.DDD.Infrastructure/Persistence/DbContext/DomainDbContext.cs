using System.Collections.Immutable;
using CleanArchitecture.DDD.Core.Helpers;
using CleanArchitecture.DDD.Core.Models;
using CleanArchitecture.DDD.Infrastructure.Exceptions;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using DatabaseContext = Microsoft.EntityFrameworkCore.DbContext;
using CleanArchitecture.DDD.Core.ExtensionMethods;

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

        if (!new DbConnectionString(_connectionString).IsReachable)
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

        var allEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .ToImmutableList();

        #region -- Soft Deletion Configuration --

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

        Expression<Func<BaseEntity, bool>> notSoftDeletedFilterExpr = bm => !bm.SoftDeleted;

        allEntityTypes
            .Where(et => et.ClrType.IsAssignableTo(typeof(BaseEntity)))
            .ForEach(entityType =>
            {
                // Modify expression to handle correct child type
                var parameter = Expression.Parameter(entityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(notSoftDeletedFilterExpr.Parameters[0], parameter, notSoftDeletedFilterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

                // Set query filter
                entityType.SetQueryFilter(lambdaExpression);
            });

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