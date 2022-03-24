using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.DDD.Domain.ValueObjects;
using StronglyTypedIds;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct DoctorID{}

public sealed class Doctor
{
    private Guid _doctorID;
    private Name _name;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public Guid DoctorID => _doctorID;

    public Name Name => _name;

    public Address Address { get; init; }
    [ForeignKey("Address")]
    public Guid AddressId;

    public Doctor()
    {
    }

    public static Doctor Create(string firstname, string? middlename, string lastname)
    {
        var name = new Name(firstname, middlename, lastname);

        return new Doctor
        {
            _doctorID = Guid.NewGuid(),
            _name = name
        };
    }

}