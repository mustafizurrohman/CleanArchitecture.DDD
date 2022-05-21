using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Retry;

namespace CleanArchitecture.DDD.API.Polly;

public class PollyPolicies
{
    public static PolicyRegistry PollyRegistry;

    public static string RetryPolicy = "RetryPolicy";
    public static string TimeOutPolicy = "TimeoutPolicy";
    public static string CircuitBreakerPolicy = "CircuitBreakerPolicy";

    public PollyPolicies()
    {
        PollyRegistry = new PolicyRegistry();
        SetupPollyRegistry();
    }

    private void SetupPollyRegistry()
    {
        PollyRegistry.Add(RetryPolicy, GetRetryPolicy());
        PollyRegistry.Add(TimeOutPolicy, GetTimeOutPolicy());
        PollyRegistry.Add(CircuitBreakerPolicy, GetCircuitBreakerPolicy());
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