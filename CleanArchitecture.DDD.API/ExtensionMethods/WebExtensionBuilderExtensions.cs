using CleanArchitecture.DDD.Infrastructure;
using System.Reflection;

namespace CleanArchitecture.DDD.API.ExtensionMethods;

public static class WebExtensionBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApplication(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((hostBuilderContext, loggerContext) =>
        {
            loggerContext.WriteTo.Console();
            loggerContext.Enrich.WithCorrelationId();
            loggerContext.Enrich.WithCorrelationIdHeader();
            loggerContext.Enrich.WithMachineName();
            loggerContext.Enrich.WithProcessId();
            loggerContext.Enrich.WithProcessName();
        });

        var connectionString = builder.Configuration.GetConnectionString("DDD_Db");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Database conntection must be specified.");

        builder.Services.AddDbContext<DomainDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            #if DEBUG
            options.EnableSensitiveDataLogging()
                .UseLoggerFactory(LoggerFactory.Create(loggerBuilder =>
                {
                    loggerBuilder.AddConsole();
                }));
            #endif
        });

        builder.Services.AddMediatR(typeof(Application.ApplicationAssemblyMarker).Assembly);

        builder.Services.AddControllers().AddNewtonsoftJson();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IValidator<Name>, NameValidator>();
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CoreAssemblyMarker>());
        builder.Services.AddValidatorsFromAssemblies(new[] { typeof(CoreAssemblyMarker).Assembly });
        
        return builder;
    }
}
