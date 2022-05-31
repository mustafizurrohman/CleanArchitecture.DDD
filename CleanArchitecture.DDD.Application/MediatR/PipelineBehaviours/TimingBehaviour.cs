using System.Diagnostics;
using Serilog;

namespace CleanArchitecture.DDD.Application.MediatR.PipelineBehaviours;

public class TimingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Stopwatch stopwatch = new();

        stopwatch.Start();
        var response = await next();
        stopwatch.Stop();

        // Log details about request which takes more than 500ms
        // Inform admin if it takes more than a second!


        Log.Information($"MediatR Timing middleware: Processed request in {stopwatch.ElapsedMilliseconds} ms.");

        return response;
    }
}