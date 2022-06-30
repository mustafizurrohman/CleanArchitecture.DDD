using AspNetHttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class HttpContextExtensions
{
    public static string GetSupportCode(this AspNetHttpContext httpContent)
    {
        var correlationId = httpContent.Request.HttpContext.Items
            .Where(item => item.Key.ToString()!.Contains("CorrelationId"))
            .Select(item => item.Value)
            .First()
            .ToString();

        return correlationId!;
    }
}