using Newtonsoft.Json;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class ObjectExtensions
{
    public static string ToFormattedJson(this object? objectInstance)
    {
        var serializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        return JsonConvert.SerializeObject(objectInstance, Formatting.Indented, serializerSettings);
    }
}