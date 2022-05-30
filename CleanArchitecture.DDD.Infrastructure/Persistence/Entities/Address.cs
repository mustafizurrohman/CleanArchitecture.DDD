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
    private Address()
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



}