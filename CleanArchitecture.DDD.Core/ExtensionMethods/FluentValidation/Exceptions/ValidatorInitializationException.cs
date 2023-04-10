namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Exceptions;

/// <inheritdoc />
public class ValidatorInitializationException : ApplicationException
{
    private string? TypeName { get; }
    private string ValidatorClassName { get; }
    private string CustomMessage { get; }

    public ValidatorInitializationException(Type typ, Exception ex) :
        base($"Could not initialize a validator for type {typ.GetTypeNameForFluentValidation()}. " +
             "Please provide a validator of type " +
             $"FluentValidation.AbstractValidator<{typ.GetTypeNameForFluentValidation()}> as a second parameter." +
             "Dependency Injection can be used for this purpose", innerException: ex)
    {
        TypeName = typ.GetTypeNameForFluentValidation();
        ValidatorClassName = typ.GetFluentValidationBaseClassName();

        CustomMessage = $"Could not initialize a validator for type {TypeName}. " +
                        $"Please provide a validator which has {ValidatorClassName} " +
                        "as base class as a second parameter." +
                        "Dependency Injection can be used for this purpose";
    }
    
    public override string Message => CustomMessage;
}
