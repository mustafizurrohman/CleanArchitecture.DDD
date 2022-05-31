using Serilog;
using Serilog.Configuration;

namespace CleanArchitecture.DDD.Core.LoggingEnrichers;

public static class EnricherExtensionMethods
{
    public static LoggerConfiguration WithReleaseNumber(this LoggerEnrichmentConfiguration enrich)
    {
        return enrich is null 
            ? throw new ArgumentNullException(nameof(enrich)) 
            : enrich.With<ReleaseNumberEnricher>();
    }

    public static LoggerConfiguration WithUsername(this LoggerEnrichmentConfiguration enrich)
    {
        return enrich is null
            ? throw new ArgumentNullException(nameof(enrich))
            : enrich.With<UserNameEnricher>();
    }

}