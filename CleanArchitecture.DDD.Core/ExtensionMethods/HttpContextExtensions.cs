using System.Diagnostics;
using AspNetHttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class HttpContextExtensions
{
    public static string GetSupportCode(this AspNetHttpContext httpContext)
    {
        var supportCode = string.Empty;

        var traceIdentifier = Activity.Current?.Id ?? httpContext.TraceIdentifier ?? string.Empty;
        var traceIdentifierParts = traceIdentifier.Split('-');

        if (traceIdentifierParts.Length >= 2)
            supportCode = traceIdentifierParts[1];

        if (supportCode == string.Empty)
            supportCode = httpContext!.TraceIdentifier;

        return supportCode!;
    }

}