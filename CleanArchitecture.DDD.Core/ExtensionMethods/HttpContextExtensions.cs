using AspNetHttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class HttpContextExtensions
{
    // Fix this: Make sure that it matches with the logged value using Serilog. 
    public static string GetSupportCode(this AspNetHttpContext httpContext)
    {
        //var correlationId = httpContext.Items
        //    .Where(item => item.Key.ToString()!.Contains("CorrelationId"))
        //    .Select(item => item.Value);

        //return correlationId?.ToString() ?? string.Empty;
        return httpContext.TraceIdentifier;
    
    }

}