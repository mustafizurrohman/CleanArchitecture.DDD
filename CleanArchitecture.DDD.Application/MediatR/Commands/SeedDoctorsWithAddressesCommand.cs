namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record SeedDoctorsWithAddressesCommand(int Num) 
    : IRequest;