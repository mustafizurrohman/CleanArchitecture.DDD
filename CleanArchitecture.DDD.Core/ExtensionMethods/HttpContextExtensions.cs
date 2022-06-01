
using AspNetHttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class HttpContextExtensions
{
    public static string GetSupportCode(this AspNetHttpContext httpContent)
    {
        return httpContent.Connection.Id;
    }
}