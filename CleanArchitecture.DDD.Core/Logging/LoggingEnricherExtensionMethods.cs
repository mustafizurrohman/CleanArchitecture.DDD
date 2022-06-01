namespace CleanArchitecture.DDD.Core.Logging;

public static class LoggingEnricherExtensionMethods
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