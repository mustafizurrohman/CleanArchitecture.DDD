using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Configurations;

internal class DoctorConfiguration 
    : IEntityTypeConfiguration<Doctor>
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
                specialization => specialization.ToStringCached(),
                specializationAsString =>  ParseSpecialization(specializationAsString)
            );

    }

    // Can be made inline but this is to demonstrate that it is possible to write complex
    // conversion logic like this
    private Specialization ParseSpecialization(string input)
    {
        return input.ToSpecialization();
    }
}