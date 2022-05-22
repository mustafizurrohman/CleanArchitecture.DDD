using CleanArchitecture.DDD.Core.Polly;
using Polly;

namespace CleanArchitecture.DDD.Application.Services;

public class SampleService : ISampleService
{
    private readonly HttpClient _httpClient;
    private readonly IPolicyHolder _policyHolder;

    public SampleService(HttpClient httpClient, IPolicyHolder policyHolder)
    {
        _httpClient = httpClient;
        _policyHolder = policyHolder;

        policyHolder.Registry
            .TryGet<IAsyncPolicy<HttpResponseMessage>>(HttpPolicyNames.HttpRetryPolicy.ToString(), out var retryPolicyFromRegistry);
    }

    public async Task TestHttpClient()
    {

    }
}