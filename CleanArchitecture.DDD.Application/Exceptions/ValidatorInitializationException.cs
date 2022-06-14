namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorInitializationException : ApplicationException
{
    private string? TypeName { get; }

    public ValidatorInitializationException(Type typ)  
    {
        TypeName = typ.FullName;
    }
    
    public override string Message => $"Could not initialize a validator for type {TypeName}. " +
                                      "Please provide a validator as a second parameter.";
}