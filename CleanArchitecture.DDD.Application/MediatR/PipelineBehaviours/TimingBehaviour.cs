using System.Diagnostics;

namespace CleanArchitecture.DDD.Application.MediatR.PipelineBehaviours;

public class TimingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // Dependency Injection can be used here
    public TimingBehaviour(DomainDbContext dbContext)
    {
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Stopwatch stopwatch = new();
        
        stopwatch.Start();
        var response = await next();
        stopwatch.Stop();

        // Log details about request which takes more than 100ms
        // Email admin if it takes more than a second!
        var requestProcessingTime = stopwatch.ElapsedMilliseconds;

        // Performance monitoring
        if (requestProcessingTime > 100)
            Log.Warning("MediatR Timing middleware: Slow request {requestType}! Took {requestProcessingTime} ms", request.GetType(), requestProcessingTime);
        else 
            Log.Information("MediatR Timing middleware: Processed request in {requestProcessingTime} ms.", requestProcessingTime);

        return response;
    }
}