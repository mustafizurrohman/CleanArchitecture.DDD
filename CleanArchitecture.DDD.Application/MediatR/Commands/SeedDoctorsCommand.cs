namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public record SeedDoctorsCommand(int Num) : IRequest;