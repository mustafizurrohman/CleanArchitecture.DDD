using System.Drawing.Printing;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace CleanArchitecture.DDD.API.ExtensionMethods;

/// <summary>
/// 
/// </summary>
public static class WebExtensionBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder ConfigureApplication(this WebApplicationBuilder builder)
    {
        builder.ConfigureSerilog()
            .ConfigureEntityFramework()
            .ConfigureServices()
            .ConfigureInputValidation()
            .ConfigureSwagger()
            .ConfigureControllers();
        
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
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "Doctor API",
                Description = "API for doctor management",
                TermsOfService = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html"),
                Contact = new OpenApiContact()
                {
                    Name = "Mustafizur Rohman",
                    Email = "mustafizur.rohman88@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/mustafizurrohman")
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.Example: \"Authorization: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.Http
            });

            var xmlFile = Assembly.GetEntryAssembly()?.GetName().Name + ".xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });


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
        // TODO: This must be an implementation detail in DomainDbContext
        var consoleLoggerFactory = LoggerFactory.Create(loggerBuilder =>
        {
            loggerBuilder
                .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddConsole();
        });

        var connectionString = builder.Configuration.GetConnectionString("DDD_Db");
        
        builder.Services.AddScoped(_ => new DomainDbContext(connectionString ?? string.Empty, true, consoleLoggerFactory));

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
                .WriteTo.File(new RenderedCompactJsonFormatter(), @"C:\dev\Serilog\logs.json", rollingInterval: RollingInterval.Minute, retainedFileCountLimit: 7);
        });

        return builder;
    }
}