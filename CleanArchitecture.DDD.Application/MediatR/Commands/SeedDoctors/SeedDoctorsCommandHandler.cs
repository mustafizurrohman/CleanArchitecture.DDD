using System.Collections.Immutable;
using CleanArchitecture.DDD.Application.Exceptions;
using CleanArchitecture.DDD.Application.MediatR.Handlers;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

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
        var faker = new Faker();

        var existingDoctorAddresses = await DbContext.Doctors
            .Select(doc => doc.AddressId)
            .ToListAsync(cancellationToken);

        var addressIds = await DbContext.Addresses
            .Select(add => add.AddressID)
            .ToListAsync(cancellationToken);

        var availableAddressIds = addressIds.Except(existingDoctorAddresses).ToList();

        if (availableAddressIds.Count < request.Num)
            throw new UniqueAddressesNotAvailable(request.Num, availableAddressIds.Count);

        var doctors = Enumerable.Range(0, request.Num)
            .Select(_ =>
            {
                var randomAddressGuid = faker.Random.ArrayElement(addressIds.ToArray());
                addressIds.Remove(randomAddressGuid);

                return Doctor.Create(randomAddressGuid);
            })
            .ToImmutableList();

        await DbContext.AddRangeAsync(doctors, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

    }
}