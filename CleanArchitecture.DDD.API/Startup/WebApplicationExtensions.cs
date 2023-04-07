using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.DDD.API.Models;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CleanArchitecture.DDD.API.Startup;

public static class WebApplicationExtensions
{
    [SuppressMessage("Usage", "ASP0014:Suggest using top level route registrations", Justification = "<Pending>")]
    public static WebApplication ConfigureHttpPipeline(this WebApplication app)
    {
        var isInDevelopment = app.Environment.IsDevelopment();

        // Configure the HTTP request pipeline.
        if (isInDevelopment)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.DisplayRequestDuration();

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(ModelRendering.Model);
                setupAction.DisplayOperationId();
                setupAction.DocExpansion(DocExpansion.List);
                setupAction.EnableDeepLinking();
            });
        }

        // TODO: Make this configurable
        app.UseCors(options =>
        {
            options.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });

        app.UseProblemDetails();
        // app.UseExceptionLoggingMiddleware();

        // app.MigrateDatabase();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseRouting();

        // GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(app.Services));

        app.UseEndpoints(endpoints =>
        {
            // endpoints.MapHangfireDashboard();
            endpoints.MapControllers();

            var mappedStatus = new Dictionary<HealthStatus, int>
            {
                {HealthStatus.Healthy, StatusCodes.Status200OK},
                {HealthStatus.Degraded, StatusCodes.Status500InternalServerError},
                {HealthStatus.Unhealthy, StatusCodes.Status503ServiceUnavailable}
            };
            
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResultStatusCodes = mappedStatus,
                ResponseWriter = WriteOverallHealthCheckResponse,
                AllowCachingResponses = false
            });

            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                ResultStatusCodes = mappedStatus,
                ResponseWriter = WriteHealthCheckResponse,
                Predicate = check => check.Tags.Contains("ready"),
                AllowCachingResponses = false
            });

            // .RequireAuthorization()
            // .RequireCors()
            // .RequireHost()
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                ResultStatusCodes = mappedStatus,
                ResponseWriter = WriteHealthCheckResponse,
                Predicate = check => check.Tags.Contains("live"),
                AllowCachingResponses = false
            });

            //endpoints.MapHealthChecks("health/ui", new HealthCheckOptions
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});
        });

        //app.UseHealthChecksUI();
        app.UseSerilogRequestLogging();

        return app;
    }

    private static Task WriteOverallHealthCheckResponse(HttpContext httpContext, HealthReport healthReport)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        var healthCheckDetailedResponse = new HealthCheckDetailedResponse(healthReport);

        return httpContext.Response.WriteAsync(healthCheckDetailedResponse.ToFormattedJsonFailSafe());
    }

    private static Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport result)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        var healthCheckResponse = new HealthCheckResponse(result);

        return httpContext.Response.WriteAsync(healthCheckResponse.ToFormattedJsonFailSafe());
    }
}