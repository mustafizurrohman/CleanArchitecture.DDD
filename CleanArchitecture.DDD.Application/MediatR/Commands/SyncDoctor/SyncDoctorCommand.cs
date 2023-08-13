namespace CleanArchitecture.DDD.Application.MediatR.Commands.SyncDoctor;

public sealed record SyncDoctorCommand(bool SimulateError)
    : IRequest;