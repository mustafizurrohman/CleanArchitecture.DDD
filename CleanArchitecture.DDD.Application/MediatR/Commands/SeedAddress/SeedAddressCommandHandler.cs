using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.MediatR.Handlers;

namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedAddress;

public sealed class SeedAddressCommandHandler
    : BaseHandler, IRequestHandler<SeedAddressCommand>
{
    public SeedAddressCommandHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task Handle(SeedAddressCommand request, CancellationToken cancellationToken)
    {
        var addresses = Enumerable.Range(0, request.Num)
            .Select(_ => Address.CreateRandom())
            .ToImmutableList();

        await DbContext.AddRangeAsync(addresses, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}