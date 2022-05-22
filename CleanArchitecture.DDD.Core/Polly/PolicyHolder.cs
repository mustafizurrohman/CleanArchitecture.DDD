using System.Net;
using System.Net.Http.Formatting;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Timeout;
using Polly.Wrap;

namespace CleanArchitecture.DDD.Core.Polly;

public enum PolicyNames
{
    TimeOutPolicy,
    RetryPolicyWithJitter
};

public enum HttpPolicyNames
{
    HttpRetryPolicy,
    HttpCircuitBreakerPolicy,
    HttpRequestFallbackPolicy
};

public enum WrappedPolicyNames
{
    TimeoutRetryAndFallbackWrap
}

public class PolicyHolder : IPolicyHolder
{
    public PolicyRegistry Registry => _pollyPolicyRegistry;

    private PolicyRegistry _pollyPolicyRegistry;
    
    public PolicyHolder()
    {
        InitPolicyRegistry();
    }

    public IAsyncPolicy<HttpResponseMessage> GetPolicy(HttpPolicyNames httpPolicyName)
    {
        return httpPolicyName switch
        {
            HttpPolicyNames.HttpRetryPolicy => GetHttpRetryPolicy(),
            HttpPolicyNames.HttpCircuitBreakerPolicy => GetHttpCircuitBreakerPolicy(),
            HttpPolicyNames.HttpRequestFallbackPolicy => GetHttpRequestFallbackPolicy(),
            _ => throw new ArgumentOutOfRangeException(nameof(httpPolicyName), httpPolicyName, null)
        };
    }

    public IAsyncPolicy GetPolicy(PolicyNames policyName)
    {
        return policyName switch
        {
            PolicyNames.RetryPolicyWithJitter => GetRetryPolicyWithJitter(),
            PolicyNames.TimeOutPolicy => GetTimeOutPolicy(),
            _ => throw new ArgumentOutOfRangeException(nameof(policyName), policyName, null)
        };
    }

    public AsyncPolicyWrap<HttpResponseMessage> GetPolicy(WrappedPolicyNames wrappedPolicyNames)
    {
        return wrappedPolicyNames switch
        {
            WrappedPolicyNames.TimeoutRetryAndFallbackWrap => GetTimeoutRetryAndFallbackWrap(),
            _ => throw new ArgumentOutOfRangeException(nameof(wrappedPolicyNames), wrappedPolicyNames, null)
        };
    }
    
    #region -- Private Methods --

    private void InitPolicyRegistry()
    {
        _pollyPolicyRegistry = new PolicyRegistry();
        SetupPollyRegistry();
    }

    private void SetupPollyRegistry()
    {
        _pollyPolicyRegistry.Add(HttpPolicyNames.HttpRetryPolicy.ToString(), GetHttpRetryPolicy());
        _pollyPolicyRegistry.Add(HttpPolicyNames.HttpCircuitBreakerPolicy.ToString(), GetHttpCircuitBreakerPolicy());
        _pollyPolicyRegistry.Add(HttpPolicyNames.HttpRequestFallbackPolicy.ToString(), GetHttpRequestFallbackPolicy());

        _pollyPolicyRegistry.Add(PolicyNames.TimeOutPolicy.ToString(), GetTimeOutPolicy());
        _pollyPolicyRegistry.Add(PolicyNames.RetryPolicyWithJitter.ToString(), GetRetryPolicyWithJitter());
    }

    private IAsyncPolicy GetRetryPolicyWithJitter()
    {
        // Reference: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly

        // Old
        // var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5);

        var maxDelay = TimeSpan.FromSeconds(45);
        
        IEnumerable<TimeSpan> delay = Backoff
            .DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 50)
            .Select(s => TimeSpan.FromTicks(Math.Min(s.Ticks, maxDelay.Ticks)));

        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(delay);
    }
    
    private IAsyncPolicy GetTimeOutPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
    }

    private IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(15));
    }

    private IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(5,
                retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(1.5, retryAttempt) * 1000),
                (_, waitingTime) =>
                {
                    Console.WriteLine("Retrying due to Polly retry policy");
                });
    }

    private IAsyncPolicy<HttpResponseMessage> GetHttpRequestFallbackPolicy()
    {
        int _cachedResult = 0;
        
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<TimeoutRejectedException>()
            .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent(_cachedResult.GetType(), _cachedResult, new JsonMediaTypeFormatter())
            });
    }

    public AsyncPolicyWrap<HttpResponseMessage> GetTimeoutRetryAndFallbackWrap()
    {
        var circuitBreakerPolicy = GetHttpCircuitBreakerPolicy();
        var fallbackPolicy = GetHttpRequestFallbackPolicy();

        return Policy.WrapAsync(circuitBreakerPolicy, fallbackPolicy);
    }

    #endregion

}