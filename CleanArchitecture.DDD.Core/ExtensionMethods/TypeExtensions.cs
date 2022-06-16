using System.Reflection;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class TypeExtensions
{
    public static string GetAssemblyName(this Type type) 
    {
        return Assembly.GetAssembly(type)?.GetName().Name ?? string.Empty;
    }

    public static string GetTypeNameForFluentValidation(this Type type)
    {
        return type.GenericTypeArguments.FirstOrDefault()?.Name ?? type.Name;
    }

    public static string GetFluentValidationBaseClassName(this Type type)
    {
        var typeName = type.GetTypeNameForFluentValidation();
        return $"FluentValidation.AbstractValidator<{typeName}>";
    }
}