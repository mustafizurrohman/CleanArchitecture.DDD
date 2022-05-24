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
