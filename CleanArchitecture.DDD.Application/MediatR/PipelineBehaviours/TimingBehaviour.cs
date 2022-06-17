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
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var displayUrl = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
        
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
            // Here we can use Weischer Email Service for Performance monitoring
            // Or Better still- Azure Application Insights?
            LogWithSpace(() => Log.Warning("MediatR Timing middleware: Slow request {requestType}! Took {requestProcessingTime} ms. Request url {displayUrl}", request.GetType(), requestProcessingTime, displayUrl));
        }
        else
        {
            LogWithSpace(() => Log.Information("MediatR Timing middleware: Processed request in {requestProcessingTime} ms.", requestProcessingTime));
        }

        return response;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void LogWithSpace(Action action)
    {
        Console.WriteLine();
        action();
        Console.WriteLine();
    }


}