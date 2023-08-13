namespace CleanArchitecture.DDD.Application.MediatR.Commands.InactivatePatient;

public sealed record InactivatePatientCommand(Guid ID)
    : IRequest;