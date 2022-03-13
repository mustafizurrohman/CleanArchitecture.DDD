using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

public record Name(string Firstname, string Middlename, string Lastname)
{
    public Name(string fistname, string lastname) : this(fistname, string.Empty, lastname)
    {
    }

    protected Name() : this(string.Empty, string.Empty, string.Empty)
    {

    }

    public override string ToString()
    {
        return (Firstname + " " + Middlename + " " + Lastname).RemoveConsequtiveSpaces();
    }
}