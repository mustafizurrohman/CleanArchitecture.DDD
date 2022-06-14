namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorInitializationException : Exception
{
    private string? TypeName { get; }
    private string CustomMessage { get; }

    public ValidatorInitializationException(Type typ)  
    {
        TypeName = typ.FullName;
        CustomMessage = $"Could not initialize a validator for type {TypeName}. " +
                        "Please provide a validator as a second parameter.";
    }

    public ValidatorInitializationException(Type typ, Exception ex) : 
        base($"Could not initialize a validator for type {typ.FullName}. Please provide a validator as a second parameter.", ex)
    {
        TypeName = typ.FullName;
        CustomMessage = $"Could not initialize a validator for type {TypeName}. " +
                        "Please provide a validator as a second parameter.";
    }

    
    public override string Message => CustomMessage;
}