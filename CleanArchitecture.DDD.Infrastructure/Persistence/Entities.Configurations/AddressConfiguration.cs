using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Configurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    { 
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(address => address.AddressID);
        }

    }
}
