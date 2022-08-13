using CleanArchitecture.DDD.Core.ExtensionMethods;
using CleanArchitecture.DDD.Domain.ValueObjects;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;
//[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson)]
//public partial struct DoctorID{}

public sealed class Doctor : BaseEntity
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public Guid DoctorID { get; set; }

    // If this is not am Empty Guid it means that it came from an external system
    // Otherwise it was inserted using the API / Empty Guid is the default value
    public Guid EDCMExternalID { get; set; }

    public Name Name { get; set; }

    // Must be init
    public Address Address { get; set; }
    [ForeignKey("Address")]
    public Guid AddressId { get; set; }

    public Specialization Specialization { get; set; }

    [NotMapped]
    public string FullName => (this.Name.Firstname + " " + this.Name.Middlename + " " + this.Name.Lastname)
        .RemoveConsecutiveSpaces();

    [NotMapped] 
    public string SpecializationAsString => Specialization.ToStringCached();

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
            Address = doctor.Address,
            Specialization = doctor.Specialization
        };
    }

    public static Doctor Create(string firstname, string? middlename, string lastname) 
    {
        return new Doctor
        {
            Name = Name.Copy(new Name(firstname, middlename, lastname))
        };
    }

    public static Doctor Create(Name name, Address address, Guid EDCMExternalID, Specialization specialization = Specialization.Unknown)
    {
        var doc = new Doctor
        {
            Name = Name.Copy(name),
            Address = Address.Copy(address),
            EDCMExternalID = EDCMExternalID,
            Specialization = specialization
        };

        return doc;
    }

    public static Doctor Create(Name name, Address address, Specialization specialization = Specialization.Unknown)
    {                  
        var doc =  new Doctor
        {
            Name = Name.Copy(name),
            Address = Address.Copy(address),
            Specialization = specialization
        };

        return doc;
    }

    public static Doctor Create(Name name, Guid addressId, Specialization specialization = Specialization.Unknown)
    {
        var doc = new Doctor
        {
            // This is not allowed: Limitation of EF Core
            // Name = name,
            // Because EF Core treats owned objects and PK-FK relations as the same
            // Should be fixed in a future version 
            // 'Copy' is workaround for now. 
            Name = Name.Copy(name),
            AddressId = addressId,
            Specialization = specialization
        };

        return doc;
    }

}