using System.Reflection;

namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorNotDefinedException : ApplicationException
{
    private string? TypeName { get; } 
    private string ValidatorClassName { get; }
    private string AssemblyName { get; }

    public ValidatorNotDefinedException(Type typ)
    {
        
        TypeName = typ.GenericTypeArguments.FirstOrDefault()?.Name ?? typ.Name;
        ValidatorClassName = $"FluentValidation.AbstractValidator<{TypeName}>";
        
        AssemblyName = typ.GenericTypeArguments.Any()
            ? GetAssemblyName(typ.GenericTypeArguments.First())
            : GetAssemblyName(typ);
    }

    private string GetAssemblyName(Type type)
    {
        return Assembly.GetAssembly(type)?.GetName().Name ?? string.Empty;
    }

    public override string Message => $"Validator for type {TypeName} not defined. " + Environment.NewLine +
                                      $"Please define a class which has {ValidatorClassName} as base class " + 
                                      $"in the Assembly '{AssemblyName}' where {TypeName} is defined  " + Environment.NewLine +
                                      "Alternatively provide the validator explicitely as an argument of the extension method.";
}