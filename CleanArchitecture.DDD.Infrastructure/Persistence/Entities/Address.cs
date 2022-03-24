using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CleanArchitecture.DDD.Infrastructure.Persistence.Entities;

public sealed class Address
{
    private string _streetAddress;
    private string _zipCode;
    private string _city;
    private string _country;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public Guid AddressID { get; init; }    
    public string StreetAddress => _streetAddress;
    public string ZipCode => _zipCode;
    public string City => _city;
    public string Country => _country;

    public Address()
    {
    }

    public static Address Create(string streetAddress, string zipCode, string city, string country)
    {
        return new Address
        {
            AddressID = Guid.NewGuid(),
            _streetAddress = streetAddress,
            _zipCode = zipCode,
            _city = city,
            _country = country
        };
    }

}
