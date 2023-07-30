namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record SyncDoctorCommand(bool SimulateError)
    : IRequest;