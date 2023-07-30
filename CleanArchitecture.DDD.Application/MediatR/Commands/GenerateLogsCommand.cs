namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record GenerateLogsCommand(int Iterations, bool WithDelay = true)
    : IRequest;