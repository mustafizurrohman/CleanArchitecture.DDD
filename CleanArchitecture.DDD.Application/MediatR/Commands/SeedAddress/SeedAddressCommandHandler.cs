using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.MediatR.Handlers;

namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedAddress;

public sealed class SeedAddressCommandHandler(IAppServices appServices) 
    : BaseHandler(appServices), IRequestHandler<SeedAddressCommand>
{
    public async Task Handle(SeedAddressCommand request, CancellationToken cancellationToken)
    {
        var addresses = Address.CreateRandom(request.Num)
            .ToImmutableList();

        await DbContext.AddRangeAsync(addresses, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}