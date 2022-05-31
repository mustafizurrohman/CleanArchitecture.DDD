﻿using System.Net;
using System.Net.Http.Formatting;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Timeout;
using Polly.Wrap;
using Serilog;

namespace CleanArchitecture.DDD.Core.Polly;


public class PolicyHolder : IPolicyHolder
{
    public PolicyRegistry Registry => _pollyPolicyRegistry;

    private PolicyRegistry _pollyPolicyRegistry;

    private readonly string dateTimeFormat = "dd.MM.yyyy HH:mm:ss";

    public PolicyHolder()
    {
        InitPolicyRegistry();
    }

    public IAsyncPolicy<HttpResponseMessage> GetPolicy(HttpPolicyNames httpPolicyName)
    {
        return httpPolicyName switch
        {
            HttpPolicyNames.HttpTimeOutPolicy => GetTimeOutPolicy().AsAsyncPolicy<HttpResponseMessage>(),
            HttpPolicyNames.HttpRetryPolicy => GetHttpRetryPolicy(),
            HttpPolicyNames.HttpCircuitBreakerPolicy => GetHttpCircuitBreakerPolicy(),
            HttpPolicyNames.HttpRequestFallbackPolicy => GetHttpRequestFallbackPolicy(),
            HttpPolicyNames.HttpRetryPolicyWithJitter => GetHttpRetryPolicyWithJitter(),
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
        _pollyPolicyRegistry.Add(HttpPolicyNames.HttpRetryPolicyWithJitter.ToString(), GetHttpRetryPolicyWithJitter());
        _pollyPolicyRegistry.Add(HttpPolicyNames.HttpNoOpPolicy.ToString(), GetNoOpPolicy());

        _pollyPolicyRegistry.Add(PolicyNames.TimeOutPolicy.ToString(), GetTimeOutPolicy());
        _pollyPolicyRegistry.Add(PolicyNames.RetryPolicyWithJitter.ToString(), GetRetryPolicyWithJitter());

        _pollyPolicyRegistry.Add(WrappedPolicyNames.TimeoutRetryAndFallbackWrap.ToString(), GetTimeoutRetryAndFallbackWrap());
    }

    private IAsyncPolicy GetRetryPolicyWithJitter()
    {
        int attempts = 0;
        
        var maxDelay = TimeSpan.FromSeconds(45);
        
        IEnumerable<TimeSpan> delay = Backoff
            .DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 50)
            .Select(s => TimeSpan.FromTicks(Math.Min(s.Ticks, maxDelay.Ticks)));

        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(delay,
                (_, waitingTime) =>
                {
                    var now = DateTime.Now;
                    var nextTry = now.Add(waitingTime);
                    Log.Information("Polly RetryAttempt# {@attempts} at {@now}. Next try at {@nextTry} if current attempt fails",
                        ++attempts,
                        now.ToString(dateTimeFormat),
                        nextTry.ToString(dateTimeFormat));
                });
    }
    
    private IAsyncPolicy GetTimeOutPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
    }

    private IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy()
    {
        // Break after 3 tries and wait for 15 seconds before retrying
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(15));
    }

    private IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy()
    {
        int attempts = 0;
        
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(5,
                retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(1.5, retryAttempt) * 1000),
                (_, waitingTime) =>
                {
                    var now = DateTime.Now;
                    var nextTry = now.Add(waitingTime);
                    Log.Information("Polly RetryAttempt# {@attempts} at {@now}. Next try at {@nextTry} if current attempt fails", 
                        ++attempts, 
                        now.ToString(dateTimeFormat), 
                        nextTry.ToString(dateTimeFormat));
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

    private AsyncPolicyWrap<HttpResponseMessage> GetTimeoutRetryAndFallbackWrap()
    {
        var circuitBreakerPolicy = GetHttpCircuitBreakerPolicy();
        var fallbackPolicy = GetHttpRequestFallbackPolicy();

        return Policy.WrapAsync(circuitBreakerPolicy, fallbackPolicy);
    }

    private IAsyncPolicy<HttpResponseMessage> GetNoOpPolicy()
    {
        return Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();
    }

    private IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicyWithJitter()
    {
        return GetRetryPolicyWithJitter().AsAsyncPolicy<HttpResponseMessage>();
    }

    #endregion

}