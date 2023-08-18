using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace CleanArchitecture.DDD.Application.MediatR.PipelineBehaviours;

public class TimingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Dependency Injection can be used here
    public TimingBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        Console.WriteLine("Initialized timing behaviour");
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Warning: This might expose sensitive data.
        var requestUrl = _httpContextAccessor.HttpContext?.Request.GetDisplayUrl();

        Stopwatch stopwatch = new();

        stopwatch.Start();
        var response = await next();
        stopwatch.Stop();

        // Log details about request which takes more than 750ms
        // Email admin if it takes more than a second!
        var requestProcessingTime = stopwatch.ElapsedMilliseconds;
        
        // Performance monitoring
        if (requestProcessingTime > 750)
        {
            // Here we can use a Email Service for Performance monitoring
            // Or Better still- Azure Application Insights?
            // Hash tag (#) before logging indicates that it is a tag. Can be used in frameworks like Stackify
            // Reference- https://stackify.com/get-smarter-log-management-with-log-tags/
            LoggingHelper.LogWithSpace(() => Log.Warning("#MediatR Timing middleware: Slow request {requestType}! Took {requestProcessingTime} ms. Request url {requestUrl}", request.GetType(), requestProcessingTime, requestUrl));
        }
        else
        {
            LoggingHelper.LogWithSpace(() => Log.Information("#MediatR Timing middleware: Processed request {requestUrl} in {requestProcessingTime} ms.", requestUrl, requestProcessingTime));
        }

        return response;
    }
    
}