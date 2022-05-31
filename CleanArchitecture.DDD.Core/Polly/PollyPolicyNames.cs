namespace CleanArchitecture.DDD.Core.Polly;

public enum PolicyNames
{
    TimeOutPolicy,
    RetryPolicyWithJitter
};

public enum HttpPolicyNames
{
    HttpNoOpPolicy,
    HttpTimeOutPolicy,
    HttpRetryPolicy,
    HttpCircuitBreakerPolicy,
    HttpRequestFallbackPolicy,
    HttpRetryPolicyWithJitter
};

public enum WrappedPolicyNames
{
    TimeoutRetryAndFallbackWrap
}
