namespace CleanArchitecture.DDD.Application.MediatR.Commands.GenerateLogs;

public sealed record GenerateLogsCommand(int Iterations, bool WithDelay = true)
    : IRequest;