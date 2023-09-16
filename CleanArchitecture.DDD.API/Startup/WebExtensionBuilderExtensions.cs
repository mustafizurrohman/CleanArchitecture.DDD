using CleanArchitecture.DDD.API.HealthCheckReporter;
using CleanArchitecture.DDD.API.Models;
using CleanArchitecture.DDD.Application;
using CleanArchitecture.DDD.Application.MediatR.PipelineBehaviours;
using CleanArchitecture.DDD.Core.Logging.CustomEnrichers;
using CleanArchitecture.DDD.Core.Logging.Helpers;
using CleanArchitecture.DDD.Core.Models;
using CleanArchitecture.DDD.Core.Polly;
using CleanArchitecture.DDD.Domain;
using Hangfire;
using Hangfire.SqlServer;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

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
            .ValidateAppSettings()
            .ConfigureEntityFramework()
            .ConfigureInputValidation()
            .ConfigureServices()
            .ConfigureSwagger()
            .ConfigureHttpClientFactory()
            .ConfigureExceptionHandling()
            .ConfigureHealthChecks()
            // .ConfigureHangfire()
            .ConfigureControllers();

        return builder;
    }

    private static WebApplicationBuilder ValidateAppSettings(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<AppSettings>()
            .Bind(builder.Configuration)
            .ValidateFluently()
            .ValidateOnStart();

        return builder;
    }

    private static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        // Note: This can be done using AspNetCore.Diagnostics.HealthChecks nuget package
        // https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
        // Custom implementation possible when class inherits from IHealthCheck
        // TODO: Refactor! Refer to Chapter 4 of
        // https://app.pluralsight.com/library/courses/implementing-cross-cutting-concerns-asp-dot-net-core-microservices/table-of-contents
        builder.Services.AddHealthChecks()
            .AddCheck("SQL Database Health Check", () =>
            {
                var connectionString = builder.GetDatabaseConnectionString();
                
                return new DbConnectionString(connectionString).IsReachable 
                    ? HealthCheckResult.Healthy() 
                    : HealthCheckResult.Unhealthy();
            }, tags: new[] { "ready" });

        //var seqUrl = builder.Configuration.GetConnectionString("Seq")!;
        //builder.Services.AddHealthChecks()
        //    .AddUrlGroup(new Uri(seqUrl), 
        //        "SEQ Health Check", 
        //        HealthStatus.Degraded, 
        //        timeout: TimeSpan.FromSeconds(15), 
        //        tags: new[] { "live" });

        // builder.Services.AddHealthChecksUI()
        //    .AddInMemoryStorage();
        
        builder.Services.Configure<HealthCheckPublisherOptions>(options =>
        {
            // Wait for 10 seconds after application start up. Default is 5 seconds
            options.Delay = TimeSpan.FromSeconds(10);
            // Run every minute
            options.Period = TimeSpan.FromSeconds(10);
            // Run all health checks
            options.Predicate = (_) => true;
            // Timeout after 20 seconds
            options.Timeout = TimeSpan.FromSeconds(20);
        });

        // Health check reports can be published by a class which inherits from IHealthCheckPublisher
        // TODO: Implement this
        // https://app.pluralsight.com/course-player?clipId=b93c3372-74d7-4623-9506-2d851b2522a9
        builder.Services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();

        return builder;
    }

    private static WebApplicationBuilder ConfigureExceptionHandling(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetailsConventions();

        builder.Services.AddProblemDetails(setup =>
        {
            setup.MapFluentValidationException();

            setup.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            setup.IncludeExceptionDetails = (httpContext, exception) => builder.Environment.IsDevelopment();
            setup.OnBeforeWriteDetails = (httpContext, details) =>
            {
                var supportCode = httpContext.GetSupportCode();

                details.Detail += $". Please use the Support Code \'{supportCode}\' for contacting us";

                Log.Error("[TICKET] Support Code: \'{supportCode}\'", supportCode);
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

        // MediatR Configuration
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        builder.Services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(TimingBehaviour<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        /*
        // TODO: Order of injection matters here. How can it be influenced?
        builder.Services.Scan(scan =>
            scan.FromAssemblyOf<ApplicationAssemblyMarker>()
                .AddClasses(classes => classes.AssignableTo(typeof(IPipelineBehavior<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
        */

        builder.Services.AddSingleton<IPolicyHolder, PolicyHolder>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<UserNameEnricher>();
        builder.Services.AddTransient<IAppServices, AppServices>();

        builder.Services
            .RegisterServicesFromAssembly<APIAssemblyMarker>();

        var excludedTypes = new List<Type> { typeof(EDCMSyncService) };
        builder.Services
            .RegisterServicesFromAssembly<ApplicationAssemblyMarker>(excludedTypes: excludedTypes);

        builder.Services.AddMemoryCache();
        builder.Services.Decorate<IDataService, DataServiceCached>();

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
                Title = "Demo API using .NET 8 (Preview)",
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
                    Array.Empty<string>()
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
        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblies(new[]
        {
            typeof(CoreAssemblyMarker).Assembly,
            typeof(ApplicationAssemblyMarker).Assembly,
            typeof(DomainAssemblyMarker).Assembly,
            typeof(APIAssemblyMarker).Assembly
        }, ServiceLifetime.Singleton);

        return builder;
    }

    private static WebApplicationBuilder ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        var connectionString = builder.GetDatabaseConnectionString();
        var loggingEnabled = builder.Environment.IsDevelopment();

        builder.Services.AddDbContextPool<DomainDbContext>(options =>
        {
            options.UseSqlServer(connectionString);

            if (!loggingEnabled) 
                return;
            
            var consoleLoggerFactory = LoggerFactory.Create(loggerBuilder =>
            {
                loggerBuilder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddConsole();
            });

            options
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(consoleLoggerFactory);

        });
        
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
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name!;

        // We can also use appSettings or any ConfigurationProvider to configure logging
        // Usage of a ConfigurationProvider is recommended
        // because it allows changes during run-time.
        builder.Host.UseSerilog((_, _, loggerConfig) =>
        {
            loggerConfig.WithSimpleConfiguration(assemblyName, builder.Configuration);
        });

        return builder;
    }

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