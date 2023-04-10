﻿using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Methods;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

internal class FirstOrLastnameValidator : AbstractValidator<FirstOrLastname>
{
    public FirstOrLastnameValidator()
    {
        SetValidationRules();
    }

    private void SetValidationRules()
    {
        RuleFor(prop => prop.Name)
            .MustBeValidName();
    }

}