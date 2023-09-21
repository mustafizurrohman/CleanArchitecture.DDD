using CSharpFunctionalExtensions;
using FluentValidation.Results;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

public sealed class Error : ValueObject
{
    private const string Separator = "||";

    public string Code { get; }
    public string Message { get; }

    internal Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    internal Error(IEnumerable<ValidationFailure> validationFailures)
    {
        Code = "Value object Validation Failure";
        Message = validationFailures
            .Select(vf => vf.ErrorMessage)
            .Aggregate((a, b) => a + Environment.NewLine + b);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Code;
    }

    public string Serialize()
    {
        return $"{Code}{Separator}{Message}";
    }

    public static Error Deserialize(string serialized)
    {
        if (serialized == "A non-empty request body is required.")
            return Errors.General.ValueIsRequired();

        string[] data = serialized.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

        if (data.Length < 2)
            throw new ArgumentException($"Invalid error serialization: '{serialized}'");

        return new Error(data[0], data[1]);
    }
}

public static class Errors
{
    public static class General
    {
        public static Error ValueIsRequired() 
            => new("value.is.required", "Value is required");

        public static Error InvalidValueObject(IEnumerable<ValidationFailure> validationFailures) 
            => new(validationFailures);
    }
}


