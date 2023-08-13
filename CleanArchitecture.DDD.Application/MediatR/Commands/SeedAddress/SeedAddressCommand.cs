namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedAddress;

public sealed record SeedAddressCommand(int Num)
    : IRequest;