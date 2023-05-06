namespace CleanArchitecture.DDD.Application.MediatR.Commands;

public sealed record SeedPatientsWithMasterDataCommand(int Num) : IRequest;