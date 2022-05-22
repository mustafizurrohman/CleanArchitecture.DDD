using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Retry;

namespace CleanArchitecture.DDD.API.Polly;

public static class PollyPolicies
{
    public static PolicyRegistry PollyPolicyRegistry;

    public static string RetryPolicy = "RetryPolicy";
    public static string TimeOutPolicy = "TimeoutPolicy";
    public static string CircuitBreakerPolicy = "CircuitBreakerPolicy";
    public static string RetryPolicyWithJitter = "RetryPolicyWithJitter";

    static PollyPolicies()
    {
        InitPolicyRegistry();    
    }

    public static void InitPolicyRegistry()
    {
        PollyPolicyRegistry = new PolicyRegistry();
        SetupPollyRegistry();
    }

    private static void SetupPollyRegistry()
    {
        PollyPolicyRegistry.Add(RetryPolicy, GetRetryPolicy());
        PollyPolicyRegistry.Add(TimeOutPolicy, GetTimeOutPolicy());
        PollyPolicyRegistry.Add(CircuitBreakerPolicy, GetCircuitBreakerPolicy());
        PollyPolicyRegistry.Add(RetryPolicyWithJitter, GetRetryPolicyWithJitter());
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(5,
                retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(1.5, retryAttempt) * 1000),
                (_, waitingTime) =>
                {
                    Console.WriteLine("Retrying due to Polly retry policy");
                });
    }

    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
    /// </summary>
    /// <returns></returns>
    private static AsyncRetryPolicy GetRetryPolicyWithJitter()
    {
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

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(15));
    }

    private static AsyncRetryPolicy GetTimeOutPolicy()
    {
        return Policy.Handle<HttpRequestException>()
            .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
            
    }
}