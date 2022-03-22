using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Configurations;

internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(doc => doc.DoctorID);
        builder.Property(doc => doc.DoctorID)
            .HasField("_doctorID")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(doc => doc.Name);
        builder.Navigation(doc => doc.Name).Metadata.SetField("_name");
        // No backing field could be found for property 'Name.DoctorID'
        // and the property does not have a getter.


    }
}