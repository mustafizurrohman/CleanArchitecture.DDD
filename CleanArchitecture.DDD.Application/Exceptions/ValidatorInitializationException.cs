﻿namespace CleanArchitecture.DDD.Application.Exceptions;

internal class ValidatorInitializationException : Exception
{
    private string? TypeName { get; }
    private string ValidatorClassName { get; }
    private string CustomMessage { get; }

    public ValidatorInitializationException(Type typ, Exception? ex = null) :
        base($"Could not initialize a validator for type {typ.FullName}. " +
             $"Please provide a validator of type FluentValidation.AbstractValidator<{typ.FullName}> as a second parameter.", ex)
    {
        TypeName = typ.FullName;
        ValidatorClassName = $"FluentValidation.AbstractValidator<{TypeName}>";
        CustomMessage = $"Could not initialize a validator for type {TypeName}. " +
                        $"Please provide a validator which has {ValidatorClassName} as base class as a second parameter.";
    }
    
    public override string Message => CustomMessage;
}
