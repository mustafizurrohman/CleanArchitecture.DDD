using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;
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

        builder.Property(doc => doc.Specialization)
            .HasDefaultValue(Specialization.Unknown);

        builder.Property(doc => doc.Specialization)
            .HasConversion(
                specialization => specialization.ToReadableString(),
                specializationAsString => (Specialization) Enum.Parse(typeof(Specialization), specializationAsString.Replace(" ", string.Empty))
            );

    }
}