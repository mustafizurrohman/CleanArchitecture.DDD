using CleanArchitecture.DDD.Application.Exceptions;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.Application.MediatR.Handlers;

public class SeedDoctorsCommandHandler : BaseHandler, IRequestHandler<SeedDoctorsCommand>
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

        var names = Enumerable.Range(0, request.Num)
            .Select(_ => Name.Create(faker.Name.FirstName(), faker.Name.LastName()))
            .ToArray();

        var doctors = Enumerable.Range(0, request.Num)
            .Select(_ =>
            {
                var randomName = faker.Random.ArrayElement(names);
                var randomAddressGuid = faker.Random.ArrayElement(addressIds.ToArray());
                var randomSpecialization = SpecializationEnumExtensions.GetRandomSpecialization();

                addressIds.Remove(randomAddressGuid);

                return Doctor.Create(randomName, randomAddressGuid, randomSpecialization);
            })
            .ToList();

        await DbContext.AddRangeAsync(doctors, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        
    }
}