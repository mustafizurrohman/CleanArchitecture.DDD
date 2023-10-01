using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.Exceptions;
using CleanArchitecture.DDD.Application.MediatR.Handlers;

namespace CleanArchitecture.DDD.Application.MediatR.Commands.SeedDoctors;

public sealed class SeedDoctorsCommandHandler
    : BaseHandler, IRequestHandler<SeedDoctorsCommand>
{
    public SeedDoctorsCommandHandler(IAppServices appServices)
        : base(appServices)
    {

    }

    public async Task Handle(SeedDoctorsCommand request, CancellationToken cancellationToken)
    {
        var existingDoctorAddresses = await DbContext.Doctors
            .Select(doc => doc.AddressId)
            .ToListAsync(cancellationToken);

        var addressIds = await DbContext.Addresses
            .Select(add => add.AddressID)
            .ToListAsync(cancellationToken);

        var availableAddressIds = addressIds
            .Except(existingDoctorAddresses)
            .ToList();

        if (availableAddressIds.Count < request.Num)
            throw new UniqueAddressesNotAvailable(request.Num, availableAddressIds.Count);

        var doctors = addressIds
            .Shuffle()
            .Take(request.Num)
            .Select(Doctor.Create)
            .ToImmutableList();

        await DbContext.AddRangeAsync(doctors, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

    }
}