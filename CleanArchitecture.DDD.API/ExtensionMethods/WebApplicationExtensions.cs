using CleanArchitecture.DDD.API.Hangfire;
using Hangfire;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CleanArchitecture.DDD.API.ExtensionMethods;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureHttpPipeline(this WebApplication app)
    {
       
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
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

        // Must be configurable in a real application
        app.UseCors(options =>
        {
            options.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });

        app.MigrateDatabase();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseRouting();

        GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(app.Services));

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHangfireDashboard();
        });

        app.UseSerilogRequestLogging();

        Log.Information("Application started ... ");

        return app;
    }   
}