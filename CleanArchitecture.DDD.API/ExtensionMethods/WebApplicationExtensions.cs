namespace CleanArchitecture.DDD.API.ExtensionMethods;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureHttpPipeline(this WebApplication app)
    {
       
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
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

        return app;
    }   
}