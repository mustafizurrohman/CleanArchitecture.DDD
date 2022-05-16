using CleanArchitecture.DDD.Domain.Exceptions;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

public record FirstOrLastname(string Name)
{
    protected FirstOrLastname() : this(string.Empty)
    {
    }

    public static FirstOrLastname Create(string firstOrLastName, bool validate = true)
    {
        var createFirstOrLastName = new FirstOrLastname()
        {
            Name = firstOrLastName
        };

        if (validate)
            Validate(createFirstOrLastName);

        return createFirstOrLastName;
    }

    public static FirstOrLastname Copy(FirstOrLastname firstOrLastname, bool validate = true)
    {
        if (validate)
            Validate(firstOrLastname);
        
        return new FirstOrLastname(firstOrLastname.Name);
    }

    public override string ToString()
    {
        return Name;
    }

    private static void Validate(FirstOrLastname firstOrLastname)
    {
        var validationResult = new FirstOrLastnameValidator().Validate(firstOrLastname);

        if (!validationResult.IsValid)
            throw new NameValidationException(validationResult.Errors);
    }

    
}
