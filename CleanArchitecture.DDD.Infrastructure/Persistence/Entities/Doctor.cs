using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.DDD.Domain.ValueObjects;
using StronglyTypedIds;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
public partial struct DoctorID{}

public sealed class Doctor
{
    //private Guid _doctorID;
    private Name _name;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public Guid DoctorID { get; set;}

    public Name Name { get; set; }

    public Address Address { get; init; }
    [ForeignKey("Address")]
    public Guid AddressId { get; set; }

    public Doctor()
    {
    }

    public static Doctor Create(string firstname, string? middlename, string lastname) 
    {
        return new Doctor
        {
            Name = Name.Copy(new Name(firstname, middlename, lastname)),
            _name = Name.Copy(new Name(firstname, middlename, lastname))
        };
    }

    public static Doctor Create(Name name, Address address)
    {                  
        var doc =  new Doctor
        {
            Name = Name.Copy(name),
            _name = Name.Copy(name),
            Address = address
        };

        return doc;
    }

    public static Doctor Create(Name name, Guid addressId)
    {
        var doc = new Doctor
        {
            // This is not allowed: Limitation of EF Core
            // Name = name,
            Name = Name.Copy(name),
            _name = Name.Copy(name),
            AddressId = addressId
        };

        return doc;
    }

}