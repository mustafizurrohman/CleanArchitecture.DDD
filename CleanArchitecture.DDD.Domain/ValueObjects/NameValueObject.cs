using CleanArchitecture.DDD.Core.ExtensionMethods;
using CSharpFunctionalExtensions;
using FluentValidation.Results;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

public class NameValueObject : ValueObject
{
    /// <summary>
    /// 
    /// </summary>
    public string Value { get; }

    private NameValueObject(string value)
    {
        Value = value;
    }

    public static Result<NameValueObject, Error> Create(string input)
    {
        var createdObject = new NameValueObject(input);

        var validator = new NameValueObjectValidator();
        var validationResult = validator.Validate(createdObject);
        
        if (!validationResult.IsValid)
            return Errors.General.InvalidValueObject(validationResult.Errors);

        return createdObject;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}

public class NameValueObjectValidator : AbstractValidator<NameValueObject>
{
    public NameValueObjectValidator()
    {
        SetValidationRules();
    }

    public void SetValidationRules()
    {
        RuleFor(prop => prop.Value)
            .MustBeValidName()
            .WithName("Name");
    }

}