namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record SeedDoctorsWithAddressesCommand(int Num, bool WithRandomDelay = false) 
    : IRequest;