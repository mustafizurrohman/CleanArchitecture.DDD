using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class ObjectExtensions
{
    public static string ToFormattedJson(this object? objectInstance)
    {
        var serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        return JsonConvert.SerializeObject(objectInstance, Formatting.Indented, serializerSettings);
    }

    public static string ToFormattedJsonFailSafe(this object o, JsonSerializerSettings? serializerSettings = null)
    {
        var stringBuilder = new StringBuilder();
        var stringWriter = new StringWriter(stringBuilder);

        using (JsonWriter textWriter = new JsonTextWriter(stringWriter))
        {
            var serializer = JsonSerializer.CreateDefault(serializerSettings);
            serializer.Formatting = Formatting.Indented;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            serializer.Serialize(textWriter, o);
        }

        return stringBuilder.ToString();
    }
}