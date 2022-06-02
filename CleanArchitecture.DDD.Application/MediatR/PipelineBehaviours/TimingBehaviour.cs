using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.DDD.Application.MediatR.PipelineBehaviours;

public class TimingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Dependency Injection can be used here
    public TimingBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        HttpRequest httpRequest = _httpContextAccessor.HttpContext.Request;
        var requestUrl = httpRequest.Scheme + "://" + httpRequest.Host + httpRequest.Path;

        Stopwatch stopwatch = new();
        
        stopwatch.Start();
        var response = await next();
        stopwatch.Stop();

        // Log details about request which takes more than 100ms
        // Email admin if it takes more than a second!
        var requestProcessingTime = stopwatch.ElapsedMilliseconds;

        // Performance monitoring
        if (requestProcessingTime > 100) 
        {
            // Here we can use Weischer Email Service for Performance monitoring
            // Or Better still- Azure Application Insights
            Console.WriteLine();
            Log.Warning("MediatR Timing middleware: Slow request {requestType}! Took {requestProcessingTime} ms. Request url {requestUrl}", request.GetType(), requestProcessingTime, requestUrl);
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine();
            Log.Information("MediatR Timing middleware: Processed request in {requestProcessingTime} ms.", requestProcessingTime);
            Console.WriteLine();
        }

        return response;
    }
}