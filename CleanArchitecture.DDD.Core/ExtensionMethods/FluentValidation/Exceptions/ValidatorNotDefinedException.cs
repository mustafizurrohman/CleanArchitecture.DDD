namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Exceptions;

public class ValidatorNotDefinedException : ApplicationException
{
    private string? TypeName { get; } 
    private string ValidatorClassName { get; }
    private string AssemblyName { get; }

    public ValidatorNotDefinedException(Type typ) 
    {

        TypeName = typ.GetTypeNameForFluentValidation();
        ValidatorClassName = typ.GetFluentValidationBaseClassName();
        
        AssemblyName = typ.GenericTypeArguments.Any()
            ? typ.GenericTypeArguments.First().GetAssemblyName()
            : typ.GetAssemblyName();
    }
    
    public override string Message => $"Validator for type {TypeName} not defined. " + Environment.NewLine +
                                      $"Please define a class which has {ValidatorClassName} as base class " + 
                                      $"in the Assembly '{AssemblyName}' where '{TypeName}' is defined  " + Environment.NewLine +
                                      "Alternatively provide the validator explicitly as an argument of the extension method.";
}