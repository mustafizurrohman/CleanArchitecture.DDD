using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

public sealed class Address
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public Guid AddressID { get; set; }    
    public string StreetAddress { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    /// <summary>
    /// Used by Entity Framework
    /// </summary>
    public Address()
    {
    }

    public Address(Address address)
    {
        Create(address.StreetAddress, address.ZipCode, address.City, address.Country);
    }
    
    public static Address Copy(Address address)
    {
        return Create(address.StreetAddress, address.ZipCode, address.City, address.Country);
    }

    // Address from AddressDTO

    public static Address Create(string streetAddress, string zipCode, string city, string country)
    {
        return new Address
        {
            AddressID = Guid.NewGuid(),
            StreetAddress = streetAddress,
            ZipCode = zipCode,
            City = city,
            Country = country
        };
    }

    public override string ToString()
    {
        const string separator = ", ";

        return StreetAddress + separator
                             + ZipCode + separator
                             + City + separator + Country;
    }

    public static bool operator !=(Address left, Address right)
    {
        return !(left == right);
    }

    public static bool operator ==(Address left, Address right)
    {
        return left.StreetAddress == right.StreetAddress
            && left.ZipCode == right.ZipCode
            && left.City == right.City
            && left.Country == right.Country;
    }

    private bool Equals(Address other)
    {
        return StreetAddress == other.StreetAddress && ZipCode == other.ZipCode && City == other.City && Country == other.Country;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Address other && Equals(other);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => HashCode.Combine(StreetAddress, ZipCode, City, Country);
    // ReSharper restore NonReadonlyMemberInGetHashCode

}