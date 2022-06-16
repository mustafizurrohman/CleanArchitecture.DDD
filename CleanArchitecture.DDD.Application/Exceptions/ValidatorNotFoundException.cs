namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorNotFoundException : ApplicationException
{
    private string? TypeName { get; } 
    private string ValidatorClassName { get; }

    public ValidatorNotFoundException(Type typ)
    {
        TypeName = typ.FullName;
        ValidatorClassName = $"FluentValidation.AbstractValidator<{TypeName}>";
    }

    public override string Message => $"Validator for type {TypeName} not defined. " +
                                      $"Please define a class which has {ValidatorClassName} as base class in the same assembly where {TypeName} is defined " +
                                      $"or provide the validator explicitely as an argument of the extension method.";
}