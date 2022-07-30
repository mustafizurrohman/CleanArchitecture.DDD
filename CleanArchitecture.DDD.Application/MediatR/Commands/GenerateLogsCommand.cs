namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public record GenerateLogsCommand(int Iterations, bool WithDelay) : IRequest;
