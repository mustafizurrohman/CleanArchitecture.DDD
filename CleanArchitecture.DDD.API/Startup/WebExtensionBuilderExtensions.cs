using CleanArchitecture.DDD.Application;
using CleanArchitecture.DDD.Core.Logging;
using CleanArchitecture.DDD.Core.Polly;
using CleanArchitecture.DDD.Domain;
using Hangfire;
using Hangfire.SqlServer;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using System.Diagnostics;

namespace CleanArchitecture.DDD.API.Startup;

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
        builder.ConfigureLogging()
            .ConfigureEntityFramework()
            .ConfigureServices()
            .ConfigureInputValidation()
            .ConfigureSwagger()
            .ConfigureHttpClientFactory()
            .ConfigureExceptionHandling()
            .ConfigureHealthChecks()
            // .ConfigureHangfire()
            .ConfigureControllers();

        return builder;
    }

    private static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        // Note: This can be done using AspNetCore.Diagnostics.HealthChecks nuget package
        // https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
        // Custom implementation possible when class inherits from IHealthCheck
        builder.Services.AddHealthChecks()
            .AddCheck("SQL Database Health Check", () =>
            {
                var connectionString = builder.GetDatabaseConnectionString();
                
                return DomainDbContext.IsDatabaseReachable(connectionString) 
                    ? HealthCheckResult.Healthy() 
                    : HealthCheckResult.Unhealthy();
            }, tags: new[] { "ready" });

        var seqUrl = builder.Configuration.GetConnectionString("Seq")!;
        builder.Services.AddHealthChecks()
            .AddUrlGroup(new Uri(seqUrl), 
                "SEQ Health Check", 
                HealthStatus.Degraded, 
                timeout: TimeSpan.FromSeconds(15), 
                tags: new[] { "live" });

        // builder.Services.AddHealthChecksUI()
        //    .AddInMemoryStorage();

        // Health check reports can be published by a class which inherits from IHealthCheckPublisher
        // TODO: Implement this
        // https://app.pluralsight.com/course-player?clipId=b93c3372-74d7-4623-9506-2d851b2522a9

        return builder;
    }

    private static WebApplicationBuilder ConfigureExceptionHandling(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(setup =>
        {
            setup.IncludeExceptionDetails = (httpContext, exception) => builder.Environment.IsDevelopment();
            setup.OnBeforeWriteDetails = (httpContext, details) =>
            {
                // TODO: Support code calculation as an extension method and a serilog enricher
                var supportCode = string.Empty;

                var traceIdentifier = Activity.Current?.Id ?? httpContext?.TraceIdentifier ?? string.Empty;
                var traceIdentifierParts = traceIdentifier.Split('-');

                if (traceIdentifierParts.Length == 4)
                    supportCode = traceIdentifierParts[1];

                if (supportCode == string.Empty)
                    supportCode = httpContext!.TraceIdentifier;

                details.Detail = "An error occurred in our API. " 
                                + $"Please use the Support Code {supportCode} for contacting us";
            };
            // setup.Rethrow<Exception>();
        });

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
        builder.Services.AddMediatR(typeof(ApplicationAssemblyMarker).Assembly);
        builder.Services.AddSingleton<IPolicyHolder, PolicyHolder>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<UserNameEnricher>();
        builder.Services.AddTransient<IAppServices, AppServices>();

        builder.Services
            .RegisterServicesFromAssembly<APIAssemblyMarker>();

        var excludedTypes = new List<Type> { typeof(EDCMSyncService) };
        builder.Services
            .RegisterServicesFromAssembly<ApplicationAssemblyMarker>(excludedTypes: excludedTypes);

        // Already injected above
        // builder.Services.AddTransient<IDataService, DataService>();
        builder.Services.AddMemoryCache();
        builder.Services.Decorate<IDataService, DataServiceCached>();

        // MediatR Configuration
        // TODO: Order of injection matters here. How can it be influenced?
        builder.Services.Scan(scan =>
            scan.FromAssemblyOf<ApplicationAssemblyMarker>()
                .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );

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
                Title = "Demo API using .NET 7 (Preview)",
                Description = "Sample API to illustrate multiple features of the .NET Framework and C#",
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
        builder.Services.AddFluentValidation();
        builder.Services.AddValidatorsFromAssemblies(new[]
        {
            typeof(CoreAssemblyMarker).Assembly,
            typeof(ApplicationAssemblyMarker).Assembly,
            typeof(DomainAssemblyMarker).Assembly
        });

        return builder;
    }

    private static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        var connectionString = builder.GetDatabaseConnectionString();
        builder.Services.AddScoped(_ => new DomainDbContext(connectionString, builder.Environment.IsDevelopment()));

        return builder;
    }

    private static WebApplicationBuilder ConfigureHangfire(this WebApplicationBuilder builder)
    {
        // Add Hangfire services.
        builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDb"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }
                ));

        // Add the processing server as IHostedService
        builder.Services.AddHangfireServer();

        return builder;
    }

    private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        // We can also use appSettings or any ConfigurationProvider to configure logging
        // Usage of a ConfigurationProvider is recommended
        // because it allows changes during run-time.
        builder.Host.UseSerilog((_, _, loggerConfig) =>
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
                .Enrich.WithTraceIdentifier()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithEnvironmentName()
                .Enrich.With<ActivityEnricher>()
                // Custom serilog enricher to append release number
                .Enrich.WithReleaseNumber()
                // Custom serilog enricher to append logged in username
                // .Enrich.WithUsername()
                .Enrich.WithProperty("Assembly", $"{assemblyName.Name}")
                .Enrich.WithProperty("Version", $"{assemblyName.Version}");

            var seqUrl = builder.Configuration.GetConnectionString("Seq")!;

            loggerConfig
                // Console configuration
                .WriteTo.Console()
                // File configuration
                .WriteTo.File(new RenderedCompactJsonFormatter(),
                    @"C:\dev\Serilog\APILogs.json",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
                // Seq Configuration
                .WriteTo.Seq(seqUrl, LogEventLevel.Debug);
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

    private static string GetDatabaseConnectionString(this WebApplicationBuilder builder)
    {
        return builder.Configuration.GetConnectionString("DDD_Db") ?? string.Empty;
    }

}