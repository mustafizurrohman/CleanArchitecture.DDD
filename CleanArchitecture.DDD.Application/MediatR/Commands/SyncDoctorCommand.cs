namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public record SyncDoctorCommand(bool SimulateError): IRequest;