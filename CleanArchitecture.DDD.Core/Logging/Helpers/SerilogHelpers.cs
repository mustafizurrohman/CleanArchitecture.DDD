using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using CleanArchitecture.DDD.Core.Logging.CustomEnrichers;
using CleanArchitecture.DDD.Core.Logging.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;

namespace CleanArchitecture.DDD.Core.Logging.Helpers;

public static class SerilogHelpers
{
    public static void WithSimpleConfiguration(this LoggerConfiguration loggerConfiguration,
        string applicationName, IConfiguration configuration)
    {
        var assemblyName = Assembly.GetEntryAssembly()!.GetName();

        var databaseConnectionString = configuration.GetConnectionString("DDD_Db") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(applicationName))
            applicationName = assemblyName.Name!;

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
                $@"C:\dev\Serilog\{applicationName}.json",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            // Seq Configuration
            .WriteTo.Seq(seqUrl, LogEventLevel.Debug);
    }

    // TODO: Refactor this!
    private static ColumnOptions GetSqlColumnOptions()
    {
        var options = new ColumnOptions();
        options.Store.Remove(StandardColumn.Message);
        options.Store.Remove(StandardColumn.MessageTemplate);
        options.Store.Remove(StandardColumn.Level);
        options.Store.Remove(StandardColumn.Exception);

        options.Store.Remove(StandardColumn.Properties);
        options.Store.Add(StandardColumn.LogEvent);
        options.LogEvent.ExcludeStandardColumns = true;
        options.LogEvent.ExcludeAdditionalProperties = true;

        options.AdditionalColumns = new Collection<SqlColumn>
            {
                new()
                { ColumnName = "PerfItem", AllowNull = false,
                    DataType = SqlDbType.NVarChar, DataLength = 100,
                    NonClusteredIndex = true },
                new()
                {
                    ColumnName = "ElapsedMilliseconds", AllowNull = false,
                    DataType = SqlDbType.Int
                },
                new()
                {
                    ColumnName = "ActionName", AllowNull = false
                },
                new()
                {
                    ColumnName = "MachineName", AllowNull = false
                }
            };

        return options;
    }

    // TODO: Refactor this!
    private static UserInfo AddCustomContextDetails(IHttpContextAccessor ctx)
    {
        var excluded = new List<string> { "nbf", "exp", "auth_time", "amr", "sub" };
        const string userIdClaimType = "sub";

        var context = ctx.HttpContext;
        var user = context?.User.Identity;
        if (user is not {IsAuthenticated: true}) return null;

        var userId = context.User.Claims.FirstOrDefault(a => a.Type == userIdClaimType)?.Value;
        var userInfo = new UserInfo
        {
            Username = user.Name!,
            UserId = userId!,
            UserClaims = new Dictionary<string, IEnumerable<string>>()
        };
        foreach (var distinctClaimType in context.User.Claims
            .Where(a => excluded.All(ex => ex != a.Type))
            .Select(a => a.Type)
            .Distinct())
        {
            userInfo.UserClaims[distinctClaimType] = context.User.Claims
                .Where(a => a.Type == distinctClaimType)
                .Select(c => c.Value)
                .ToList();
        }

        return userInfo;
    }

}