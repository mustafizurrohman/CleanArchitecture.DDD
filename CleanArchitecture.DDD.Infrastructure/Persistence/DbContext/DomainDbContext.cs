﻿using DatabaseContext = Microsoft.EntityFrameworkCore.DbContext;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;

public partial class DomainDbContext : DatabaseContext
{
    private readonly string _connectionString;
    private readonly bool _useLogger;
    
    private DomainDbContext()
    {
    }

    public DomainDbContext(string connectionString, bool useLogger)
    {
        _connectionString = connectionString;

        if (!IsConnectionStringValid(_connectionString))
            throw new Exception("Invalid database connection string or database is not reachable ... ");

        _useLogger = useLogger;
    }

    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }

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
        
        // TODO: Can this be done using reflection?
        modelBuilder.Entity<Doctor>()
            .HasQueryFilter(doc => !doc.SoftDeleted);

        modelBuilder.Entity<Address>()
            .HasQueryFilter(addr => !addr.SoftDeleted);

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

    private void SetAuditingData()
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
    }

    private bool IsConnectionStringValid(string connectionString)
    {
        string RemoveDatabaseFromConnectionString(string connStr)
        {
            return connStr
                .Split(";")
                .Where(str => !str.Contains("Database="))
                .Aggregate((a, b) => a + ";" + b);
        }

        if (string.IsNullOrWhiteSpace(connectionString))
            return false;

        connectionString = RemoveDatabaseFromConnectionString(connectionString);

        SqlConnection? connection = null;

        try
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        catch (Exception)
        {
            Log.Fatal("Database not reachable ... ");
            return false;
        }
        finally
        {
            if (connection?.State == ConnectionState.Open)
                connection.Close();
        }
        
        return true;
    }
    
}