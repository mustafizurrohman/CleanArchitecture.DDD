using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Events;

namespace CleanArchitecture.DDD.Core.Logging;

public class ReleaseNumberEnricher : ILogEventEnricher
{
    LogEventProperty _cachedProperty;

    private const string PropertyName = "ReleaseNumber";

    /// <summary>
    /// Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
    }

    private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
    {
        return _cachedProperty;
    }

    // Qualify as uncommon-path
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
    {
        var value = Environment.GetEnvironmentVariable("RELEASE_NUMBER") ?? "local";
        return propertyFactory.CreateProperty(PropertyName, value);
    }
}