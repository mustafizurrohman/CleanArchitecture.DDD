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
        // Thread.Sleep(700);
        var response = await next();
        stopwatch.Stop();

        // Log details about request which takes more than 500ms
        // Email admin if it takes more than a second!
        var requestProcessingTime = stopwatch.ElapsedMilliseconds;

        if (requestProcessingTime > 500)
        {
            Log.Warning("Slow request {requestType}. Took {requestProcessingTime} ms", request.GetType(), requestProcessingTime);
        }

        Log.Information($"MediatR Timing middleware: Processed request in {requestProcessingTime} ms.");

        return response;
    }
}