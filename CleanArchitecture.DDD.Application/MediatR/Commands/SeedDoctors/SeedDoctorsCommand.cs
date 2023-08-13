namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedDoctors;

public sealed record SeedDoctorsCommand(int Num)
    : IRequest;