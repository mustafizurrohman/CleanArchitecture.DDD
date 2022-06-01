namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public record SeedAddressCommand(int Num) : IRequest;
