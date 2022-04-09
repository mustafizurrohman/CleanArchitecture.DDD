using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Configurations;

internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{ 
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(addr => addr.AddressID);

        //builder.Property(addr => addr.StreetAddress)
        //    .HasField("_streetAddress")
        //    .UsePropertyAccessMode(PropertyAccessMode.Field);

        //builder.Property(addr => addr.ZipCode)
        //    .HasField("_zipCode")
        //    .UsePropertyAccessMode(PropertyAccessMode.Field);

        //builder.Property(addr => addr.City)
        //    .HasField("_city")
        //    .UsePropertyAccessMode(PropertyAccessMode.Field);

        //builder.Property(addr => addr.Country)
        //    .HasField("_country")
        //    .UsePropertyAccessMode(PropertyAccessMode.Field);
            
    }

}
