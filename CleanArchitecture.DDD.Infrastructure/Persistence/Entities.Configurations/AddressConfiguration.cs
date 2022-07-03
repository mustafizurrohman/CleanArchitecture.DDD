using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Configurations;

internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{ 
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(address => address.AddressID);

        var addr = GetAddresses();
        builder.HasData(addr);
    }

    public IEnumerable<Address> GetAddresses()
    {
        var fileContents = File.ReadAllText("../CleanArchitecture.DDD.Infrastructure/Seed/address.json");

        var addressList = JsonConvert.DeserializeObject<IEnumerable<Address>>(fileContents);

        addressList = addressList
            .ToList();

        return addressList;
    }

}