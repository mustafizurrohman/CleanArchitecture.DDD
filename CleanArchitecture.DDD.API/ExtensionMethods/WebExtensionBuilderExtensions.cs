using System.Configuration;
using System.Reflection;
using CleanArchitecture.DDD.API.Hangfire;
using CleanArchitecture.DDD.Application.Services;
using CleanArchitecture.DDD.Core.Polly;
using CleanArchitecture.DDD.Domain;
using Hangfire;
using Hangfire.SqlServer;
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
            .ConfigureHttpClientFactory()
            .ConfigureHangfire()
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
        builder.Services.AddSingleton<IPolicyHolder, PolicyHolder>();

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

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIx...\"",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            var xmlFile = Assembly.GetEntryAssembly()?.GetName().Name + ".xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.EnableAnnotations();
        });


        return builder;
    }

    private static WebApplicationBuilder ConfigureInputValidation(this WebApplicationBuilder builder)
    {
        // builder.Services.AddSingleton<IValidator<Name>, NameValidator>();
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CoreAssemblyMarker>());
        builder.Services.AddValidatorsFromAssemblies(new[] { typeof(CoreAssemblyMarker).Assembly });
        builder.Services.AddValidatorsFromAssemblies(new[] { typeof(DomainAssemblyMarker).Assembly });

        return builder;
    }

    private static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DDD_Db") ?? string.Empty;
        
        builder.Services.AddScoped(_ => new DomainDbContext(connectionString, builder.Environment.IsDevelopment()));

        return builder;
    }

    private static WebApplicationBuilder ConfigureHangfire(this WebApplicationBuilder builder)
    {
        GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(builder.Services.BuildServiceProvider()));

        // Add Hangfire services.
        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDb"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        // Add the processing server as IHostedService
        builder.Services.AddHangfireServer();

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
                .MinimumLevel.Override("IdentityServer4", LogEventLevel.Information)
                .MinimumLevel.Override("Hangfire", LogEventLevel.Information);

            loggerConfig
                .Enrich.WithCorrelationId()
                .Enrich.WithCorrelationIdHeader()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty("Assembly", $"{assemblyName.Name}")
                .Enrich.WithProperty("Version", $"{assemblyName.Version}");

            loggerConfig
                .WriteTo.Console()
                .WriteTo.File(new RenderedCompactJsonFormatter(), 
                    @"C:\dev\Serilog\logs.json", 
                    rollingInterval: RollingInterval.Day, 
                    retainedFileCountLimit: 7);
        });

        return builder;
    }

    // TODO: Ensure that best practice is followed here
    private static WebApplicationBuilder ConfigureHttpClientFactory(this WebApplicationBuilder builder)
    {
        var policyHolder = new PolicyHolder();
        var retryPolicy = policyHolder.GetPolicy(HttpPolicyNames.HttpRetryPolicy);
        
        builder.Services.AddHttpClient<IEDCMSyncService, EDCMSyncService>(config =>
            {
                config.BaseAddress = new Uri("https://localhost:7125/");
                config.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
            })
            // .SetHandlerLifetime(TimeSpan.FromMinutes(1))
            .AddPolicyHandler(retryPolicy);

        return builder;
    }

}