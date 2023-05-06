namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record SeedAddressCommand(int Num): IRequest;