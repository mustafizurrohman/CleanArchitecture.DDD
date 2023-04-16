namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public record InactivatePatientCommand(Guid ID) : IRequest;
