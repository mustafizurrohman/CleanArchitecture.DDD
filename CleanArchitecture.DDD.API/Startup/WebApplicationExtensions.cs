using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CleanArchitecture.DDD.API.Startup;

public static class WebApplicationExtensions
{
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

        app.MigrateDatabase();

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

        //TODO: Use a class here
        var json = new JObject(
            new JProperty("OverallStatus", healthReport.Status.ToString()),
            new JProperty("TotalChecksDuration", healthReport.TotalDuration.TotalSeconds.ToString("0:0.00")),
            new JProperty("DependencyHealthChecks", new JObject(healthReport.Entries.Select(dicItem =>
                new JProperty(dicItem.Key, new JObject(
                    new JProperty("Status", dicItem.Value.Status.ToString()),
                    new JProperty("Duration", dicItem.Value.Duration.TotalSeconds.ToString("0:0.00")),
                    new JProperty("Exception", dicItem.Value.Exception?.Message),
                    new JProperty("Data", new JObject(dicItem.Value.Data.Select(dicData =>
                        new JProperty(dicData.Key, dicData.Value))))
                ))
            )))
        );

        return httpContext.Response.WriteAsync(json.ToFormattedJson());
    }

    private static Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport result)
    {
        httpContext.Response.ContentType = "application/json";

        //TODO: Use a class here
        var json = new JObject(
            new JProperty("OverallStatus", result.Status.ToString()),
            new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00"))
        );

        return httpContext.Response.WriteAsync(json.ToFormattedJson());
    }
}