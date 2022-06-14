namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorNotFoundException : ApplicationException
{
    private Type TypeName { get; } 

    public ValidatorNotFoundException(Type typ)
    {
        TypeName = typ;
    }

    public override string Message => $"Validator for type {nameof(TypeName)} not found.";
}