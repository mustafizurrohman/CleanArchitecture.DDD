﻿namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        var dataAsString = JsonSerializer.Serialize(data);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);

        return httpClient.PostAsync(url, content);
    }

    public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        var dataAsString = JsonSerializer.Serialize(data);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);

        return httpClient.PutAsync(url, content);
    }

    public static async Task<T?> ReadContentAs<T>(this HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        
        var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}