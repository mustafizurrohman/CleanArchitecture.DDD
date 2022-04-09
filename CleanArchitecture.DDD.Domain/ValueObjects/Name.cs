using CleanArchitecture.DDD.Core.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

public record Name(string Firstname, string? Middlename, string Lastname)
{
    public Name(string fistname, string lastname) : this(fistname, string.Empty, lastname)
    {
    }


    public Name() : this(string.Empty, string.Empty, string.Empty)
    {

    }

    public override string ToString()
    {
        return (Firstname + " " + Middlename + " " + Lastname).RemoveConsequtiveSpaces();
    }

    public static Name Copy(Name name)
    { 
        return new Name(name.Firstname, name.Lastname);
    }
}