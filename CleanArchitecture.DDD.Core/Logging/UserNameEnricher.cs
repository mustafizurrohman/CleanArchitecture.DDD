using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace CleanArchitecture.DDD.Core.Logging;

// TODO: Debug this!
public class UserNameEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private const string PropertyName = "USERNAME";

    public UserNameEnricher() : this(new HttpContextAccessor())
    {

    }

    public UserNameEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var username = "Unknown";

        if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
        {
            // Access the name of the logged-in user
            username = _httpContextAccessor.HttpContext.User.Identity.Name ?? "Unknown";
        }

        var userNameProperty = propertyFactory.CreateProperty(PropertyName, username);
        logEvent.AddPropertyIfAbsent(userNameProperty);
    }

}