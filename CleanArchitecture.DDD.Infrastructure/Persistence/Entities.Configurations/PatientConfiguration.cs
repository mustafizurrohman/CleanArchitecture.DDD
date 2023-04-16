using CleanArchitecture.DDD.Core.ExtensionMethods;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Configurations;

internal class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.OwnsOne(prop => prop.MasterData, formatter => { formatter.ToJson(); });
    }
}

