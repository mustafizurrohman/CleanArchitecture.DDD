using Bogus;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

public sealed class Address 
    : BaseEntity
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

    public static Address CreateRandom()
    {
        Faker faker = new();

        IReadOnlyList<string> fakeCities = new List<string>()
        {
            "Berlin", "Bern", "Vienna", "Hamburg", "Köln", "Munich", "Stuttgart",
            "Zurich", "Graz", "Bonn", "Göttingen", "Rome", "Venice", "Hannover"
        };

        IReadOnlyList<string> fakeCountries = new List<string>()
        {
            "Deutschland", "Osterreich", "Schweiz"
        };

        var address = Create(faker.Address.StreetName(), faker.Address.ZipCode(), faker.Random.ArrayElement(fakeCities.ToArray()), faker.Random.ArrayElement(fakeCountries.ToArray()));

        return address;
    }

    public void UpdateAddress(Address updatedAddress)
    {
        StreetAddress = updatedAddress.StreetAddress;
        City = updatedAddress.City;
        Country = updatedAddress.Country;
        ZipCode = updatedAddress.ZipCode;
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
        return StreetAddress == other.StreetAddress 
               && ZipCode == other.ZipCode 
               && City == other.City 
               && Country == other.Country;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Address other && Equals(other);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => HashCode.Combine(StreetAddress, ZipCode, City, Country);
    // ReSharper restore NonReadonlyMemberInGetHashCode

}