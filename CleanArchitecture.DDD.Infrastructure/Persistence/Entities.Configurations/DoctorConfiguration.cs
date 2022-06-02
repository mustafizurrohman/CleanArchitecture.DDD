using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Configurations;

internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(doc => doc.DoctorID);

        builder.OwnsOne(doc => doc.Name);

        builder.Property(doc => doc.EDCMExternalID)
            .HasDefaultValue(Guid.Empty);
    }
}