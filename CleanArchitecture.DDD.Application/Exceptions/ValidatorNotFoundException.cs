using CleanArchitecture.DDD.Domain.ValueObjects;

namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorNotFoundException : ApplicationException
{
    private string? TypeName { get; } 
    private string ClassName { get; }

    public ValidatorNotFoundException(Type typ)
    {
        TypeName = typ.FullName;
        ClassName = $"AbstractValidator<{typ.FullName}>";
    }

    public override string Message => $"Validator for type {TypeName} not found. Please create a class which inherits from {ClassName}";
}