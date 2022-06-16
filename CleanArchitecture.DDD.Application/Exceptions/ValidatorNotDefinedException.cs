namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorNotDefinedException : ApplicationException
{
    private string? TypeName { get; } 
    private string ValidatorClassName { get; }

    public ValidatorNotDefinedException(Type typ)
    {
        TypeName = typ.GenericTypeArguments.FirstOrDefault()?.Name ?? typ.Name;
        ValidatorClassName = $"FluentValidation.AbstractValidator<{TypeName}>";
    }

    public override string Message => $"Validator for type {TypeName} not defined. " +
                                      $"Please define a class which has {ValidatorClassName} as base class in the same assembly where {TypeName} is defined " +
                                      $"or provide the validator explicitely as an argument of the extension method.";
}