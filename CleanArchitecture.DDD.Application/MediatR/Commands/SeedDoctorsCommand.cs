namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record SeedDoctorsCommand(int Num): IRequest;