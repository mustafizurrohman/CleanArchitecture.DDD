using System.Reflection;
using CleanArchitecture.DDD.Core.Logging.CustomEnrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace CleanArchitecture.DDD.Core.Logging.Helpers;

public static class SerilogHelpers
{
    public static void WithSimpleConfiguration(this LoggerConfiguration loggerConfiguration,
        string applicationName, IConfiguration configuration)
    {
        var assemblyName = Assembly.GetEntryAssembly()!.GetName();

        loggerConfiguration
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("IdentityServer4", LogEventLevel.Information)
            .MinimumLevel.Override("Hangfire", LogEventLevel.Information);

        loggerConfiguration
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

        var seqUrl = configuration.GetConnectionString("Seq")!;

        loggerConfiguration
            // Console configuration
            .WriteTo.Console()
            // File configuration
            .WriteTo.File(new RenderedCompactJsonFormatter(),
                @"C:\dev\Serilog\APILogs.json",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            // Seq Configuration
            .WriteTo.Seq(seqUrl, LogEventLevel.Debug);
    }
}