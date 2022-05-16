using System.Data;
using System.Reflection;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using DatabaseContext = Microsoft.EntityFrameworkCore.DbContext;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;

public partial class DomainDbContext : DatabaseContext
{
    private readonly string _connectionString;
    private readonly bool _useLogger;
    private readonly ILoggerFactory _loggerFactory;
    
    private DomainDbContext()
    {
    }

    public DomainDbContext(string connectionString, bool useLogger, ILoggerFactory loggerFactory)
    {
        _connectionString = connectionString;

        if (!IsConnectionStringValid(_connectionString))
            throw new Exception("Invalid database connection string or database is not reachable ... ");

        _useLogger = useLogger;
        _loggerFactory = loggerFactory;
    }

    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);

        if (_useLogger)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(_loggerFactory);
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