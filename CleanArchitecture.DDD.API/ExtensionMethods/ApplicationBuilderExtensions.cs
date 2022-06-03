using System.Net;
using System.Text;
using System.Text.Json;
using CleanArchitecture.DDD.Core.ExtensionMethods;
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
        using IServiceScope? serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

        if (serviceScope is null) 
            return;
            
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
            Log.Information("Database Migration completed ...");
        }
    }

    /// <summary>
    /// Custom Exception Handler 
    /// This can also be implemented as a Middleware
    /// </summary>
    /// <param name="applicationBuilder"></param>
    public static void UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature?.Error ?? new Exception();

                // Log all details of the unhandled exception here
                Log.Error(exception, exception.Message);

                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                // The user only gets a support code and no details of the internal support error. 
                // We can use this code to filter out the logs for this specific request
                // Using Kibana? or by explicitely saving this code in Database 
                var errorMessage = "An internal server error occured. Support code : \'" + context.GetSupportCode() + "\'";

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorMessage), Encoding.UTF8);
            });
        });
    }
}