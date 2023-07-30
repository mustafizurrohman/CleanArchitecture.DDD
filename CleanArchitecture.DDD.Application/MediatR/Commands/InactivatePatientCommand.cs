namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record InactivatePatientCommand(Guid ID) 
    : IRequest;