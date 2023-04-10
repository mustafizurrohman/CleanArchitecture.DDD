﻿using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
#pragma warning disable S1133

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class ObjectExtensions
{

    // For demo only. Disabling pragma S1133 to avoid warning
    [Obsolete("Please use \'ToFormattedJsonFailSafe()\' to avoid OutOfMemoryException")]
    public static string ToFormattedJson(this object? objectInstance,JsonSerializerSettings? serializerSettings = null)
    {
        var serializer = JsonSerializer.CreateDefault(serializerSettings);
        serializer.Formatting = Formatting.Indented;
        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        return JsonConvert.SerializeObject(objectInstance, Formatting.Indented, serializerSettings);
    }

    public static string ToFormattedJsonFailSafe(this object? objectInstance, JsonSerializerSettings? serializerSettings = null)
    {
        var stringBuilder = new StringBuilder();
        var stringWriter = new StringWriter(stringBuilder);

        using JsonWriter textWriter = new JsonTextWriter(stringWriter);

        var serializer = JsonSerializer.CreateDefault(serializerSettings);
        serializer.Formatting = Formatting.Indented;
        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        serializer.Serialize(textWriter, objectInstance);

        return stringBuilder.ToString();
    }
}