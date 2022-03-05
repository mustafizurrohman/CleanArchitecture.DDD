

namespace CleanArchitecture.DDD.Domain.ValueObjects;

/// <summary>
/// 
/// </summary>
public class Firstname : ValueOf<string, Firstname>
{
    protected override void Validate()
    {
        var validator = new FirstnameValidator();
        var validationResults = validator.Validate(this);
        
        if (!validationResults.IsValid)
            throw new Exception(validationResults.ToErrorString());
        
    }
}

internal class FirstnameValidator : AbstractValidator<Firstname>
{
    public FirstnameValidator()
    {
        RuleFor(x => x.Value).MustBeValidName();
    }
}