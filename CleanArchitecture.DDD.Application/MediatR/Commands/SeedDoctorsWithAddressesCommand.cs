namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public record SeedDoctorsWithAddressesCommand(int Num) : IRequest;
