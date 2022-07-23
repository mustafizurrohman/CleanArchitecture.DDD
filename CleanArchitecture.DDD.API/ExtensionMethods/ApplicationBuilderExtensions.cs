using CleanArchitecture.DDD.API.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace CleanArchitecture.DDD.API.ExtensionMethods;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Migrates the database.
    /// </summary>
    /// <param name="applicationBuilder">The application builder.</param>
    public static void MigrateDatabase(this IApplicationBuilder applicationBuilder)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        using IServiceScope? serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

        if (serviceScope is null) 
            return;
        
        // TODO: Debug and fix the exception thrown here ... 
        using var context = serviceScope.ServiceProvider.GetService<DomainDbContext>();
        context?.Database.EnsureCreated();
        try
        {
            Log.Information("Starting database migration ...");
            context?.Database.Migrate();
        }
        catch (Exception ex)
        {
            Log.Error("An exception occurred during migrating database ...");
            Log.Error(ex, ex.Message);
        }
        finally
        {
            stopwatch.Stop();
            Log.Information("Database Migration completed in {TimeTakenForDbMigration} milliseconds ...", stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Custom Exception Handler 
    /// This can also be implemented as a Middleware
    /// </summary>
    /// <param name="applicationBuilder"></param>
    public static void UseExceptionLoggingMiddleware(this WebApplication applicationBuilder)
    {
        applicationBuilder.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(context =>
            {
                Console.WriteLine("In Exception Logging Middleware ... ");

                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature?.Error ?? new Exception();

                Log.Error(exception, exception.Message);
                return Task.CompletedTask;
            });
        });
    }
}