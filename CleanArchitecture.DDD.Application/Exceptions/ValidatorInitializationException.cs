namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorInitializationException : Exception
{
    private string? TypeName { get; }
    private string ValidatorClassName { get; }
    private string CustomMessage { get; }

    public ValidatorInitializationException(Type typ, Exception? ex = null) :
        base($"Could not initialize a validator for type {typ.GetTypeNameForFluentValidation()}. " +
             "Please provide a validator of type " +
             $"FluentValidation.AbstractValidator<{typ.GetTypeNameForFluentValidation()}> as a second parameter." +
             "Dependency Injection can be used for this purpose", ex)
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
