using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

public record Name(string Firstname, string? Middlename, string Lastname) 
{
    public Name(string firstname, string lastname) : this(firstname, string.Empty, lastname)
    {
    }

    public Name() : this(string.Empty, string.Empty, string.Empty)
    {
    }

    public Name(Name name)
    {
        Create(name.Firstname, name.Middlename ?? string.Empty, name.Lastname);
    }

    public static Name Create(string firstName, string middleName, string lastName)
    {
        return new Name
        {
            Firstname = firstName,
            Middlename = middleName,
            Lastname = lastName
        };
    }

    public override string ToString()
    {
        return (Firstname + " " + Middlename + " " + Lastname).RemoveConsequtiveSpaces();
    }
       
    public static Name Copy(Name name)
    { 
        return new Name(name.Firstname, name.Middlename ?? string.Empty, name.Lastname);
    }
}