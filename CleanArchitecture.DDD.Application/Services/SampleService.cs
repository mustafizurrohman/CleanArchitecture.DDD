using CleanArchitecture.DDD.Core.Polly;
using Polly;

namespace CleanArchitecture.DDD.Application.Services;

public class SampleService : ISampleService
{
    private readonly HttpClient _httpClient;
    private readonly IPolicyHolder _policyHolder;

    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    public SampleService(HttpClient httpClient, IPolicyHolder policyHolder)
    {
        _httpClient = httpClient;
        _policyHolder = policyHolder;
        

        policyHolder.Registry
            .TryGet<IAsyncPolicy<HttpResponseMessage>>(HttpPolicyNames.HttpRetryPolicy.ToString(), out _retryPolicy);
        
    }

    public async Task<IEnumerable<Doctor>> TestHttpClient()
    {
        var response = await _httpClient.GetAsync("Fake/doctors");

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsAsync<IEnumerable<Doctor>>();

        throw new TimeoutException("Error while retrieving data from remote server ...");
    }
}