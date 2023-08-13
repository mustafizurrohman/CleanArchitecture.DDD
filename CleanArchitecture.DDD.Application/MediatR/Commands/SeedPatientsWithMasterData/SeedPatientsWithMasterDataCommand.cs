namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedPatientsWithMasterData;

public sealed record SeedPatientsWithMasterDataCommand(int Num)
    : IRequest;