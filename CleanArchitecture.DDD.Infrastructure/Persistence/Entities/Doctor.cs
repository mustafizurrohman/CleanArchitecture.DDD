using CleanArchitecture.DDD.Domain.ValueObjects;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
//[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
//public partial struct DoctorID{}

public sealed class Doctor
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public Guid DoctorID { get; set; }

    public Guid EDCMExternalID { get; set; }

    public Name Name { get; set; }

    // Must be init
    public Address Address { get; set; }
    [ForeignKey("Address")]
    public Guid AddressId { get; set; }

    /// <summary>
    /// Required for EntityFramework
    /// </summary>
    public Doctor()
    {
    }

    public Doctor UpdateAddress(Address updatedAddress)
    {
        this.Address = updatedAddress;
        return this;
    }

    public static Doctor Copy(Doctor doctor)
    {
        return new Doctor()
        {
            DoctorID = doctor.DoctorID,
            Name = doctor.Name,
            Address = doctor.Address
        };
    }

    public static Doctor Create(string firstname, string? middlename, string lastname) 
    {
        return new Doctor
        {
            Name = Name.Copy(new Name(firstname, middlename, lastname))
        };
    }

    public static Doctor Create(Name name, Address address, Guid EDCMExternalID)
    {
        var doc = new Doctor
        {
            Name = Name.Copy(name),
            Address = Address.Copy(address),
            EDCMExternalID = EDCMExternalID
        };

        return doc;
    }

    public static Doctor Create(Name name, Address address)
    {                  
        var doc =  new Doctor
        {
            Name = Name.Copy(name),
            Address = Address.Copy(address)
        };

        return doc;
    }

    public static Doctor Create(Name name, Guid addressId)
    {
        var doc = new Doctor
        {
            // This is not allowed: Limitation of EF Core
            // Name = name,
            // Because EF Core treats owned objects and PK-FK relations as the same
            // Should be fixed in a future version 
            // 'Copy' is workaround for now. 
            Name = Name.Copy(name),
            AddressId = addressId
        };

        return doc;
    }

}