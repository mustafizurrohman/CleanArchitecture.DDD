﻿namespace CleanArchitecture.DDD.API.ExtensionMethods;

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

                setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
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

        app.MapControllers();

        app.UseSerilogRequestLogging();

        Log.Information("Application started ... ");

        return app;
    }   
}