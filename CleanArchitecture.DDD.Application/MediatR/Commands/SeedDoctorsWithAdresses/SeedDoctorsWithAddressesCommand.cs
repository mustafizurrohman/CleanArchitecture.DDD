namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedDoctorsWithAdresses;

public sealed record SeedDoctorsWithAddressesCommand(int Num, bool WithRandomDelay = false)
    : IRequest;