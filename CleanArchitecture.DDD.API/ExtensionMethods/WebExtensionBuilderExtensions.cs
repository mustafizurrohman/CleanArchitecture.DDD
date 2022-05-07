using System.Drawing.Printing;
using System.Reflection;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace CleanArchitecture.DDD.API.ExtensionMethods;

public static class WebExtensionBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApplication(this WebApplicationBuilder builder)
    {
        builder.ConfigureSerilog()
            .ConfigureEntityFramework()
            .ConfigureServices()
            .ConfigureControllers()
            .ConfigureSwagger()
            .ConfigureInputValidation();
        
        return builder;
    }


    private static WebApplicationBuilder ConfigureControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddNewtonsoftJson();

        return builder;
    }

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddMediatR(typeof(Application.ApplicationAssemblyMarker).Assembly);

        return builder;
    }

    private static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    private static WebApplicationBuilder ConfigureInputValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IValidator<Name>, NameValidator>();
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CoreAssemblyMarker>());
        builder.Services.AddValidatorsFromAssemblies(new[] { typeof(CoreAssemblyMarker).Assembly });

        return builder;
    }

    private static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DDD_Db");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Database connection must be specified.");

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


        return builder;
    }

    private static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((provider, contextBoundObject, loggerConfig) =>
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            loggerConfig
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("IdentityServer4", LogEventLevel.Information);

            loggerConfig
                .Enrich.WithCorrelationId()
                .Enrich.WithCorrelationIdHeader()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithProperty("Assembly", $"{assemblyName.Name}")
                .Enrich.WithProperty("Version", $"{assemblyName.Version}");

            loggerConfig
                .WriteTo.Console()
                .WriteTo.File(new RenderedCompactJsonFormatter(), @"C:\dev\Serilog\logs.json", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7);
        });

        return builder;
    }
}