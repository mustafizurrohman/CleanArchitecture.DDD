﻿using CleanArchitecture.DDD.API.Models;
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
    /// <param name="isDevelopment"></param>
    /// TODO: Just log the exception here and do nothing else. UseProblemDetails() takes care of the rest.
    public static void UseCustomExceptionHandler(this WebApplication applicationBuilder, bool? isDevelopment = null)
    {
        var isInDevelopment = isDevelopment ?? applicationBuilder.Environment.IsDevelopment();

        applicationBuilder.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature?.Error ?? new Exception();

                context.Response.ContentType = MediaTypeNames.Application.Json;

                // TaskCanceledException Handling
                if (exception is TaskCanceledException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    var message = "Request was cancelled";
                    
                    await context.Response.WriteAsync(message.ToFormattedJson(), Encoding.UTF8);
                }

                // Log all details of the unhandled exception here
                Log.Error(exception, exception.Message);

                // TODO: May be we want to inform the Admin here using the Mailer Service?
                // Or as a message in Teams using Serilog.Sinks.MicrosoftTeams

                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                
                // The user only gets a support code and no details of the internal server exception. 
                // We can use this code to filter out the logs for this specific request
                // Using Kibana? or by explicitly saving this code in Database while logging 
                var supportCode = context.GetSupportCode();
                var productionErrorMessage =
                    "An unexpected error occured. Please contact support with code \'" + supportCode + "\'.";

                // var x = exception.ToProblemDetails();
                
                var response = isInDevelopment
                    ? new ExceptionReportModel(exception, supportCode).ToFormattedJson()
                    : productionErrorMessage.ToFormattedJson();

                await context.Response.WriteAsync(response, Encoding.UTF8);
            });
        });
    }
}