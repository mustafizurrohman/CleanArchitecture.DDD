using Microsoft.Extensions.Options;
using CleanArchitecture.DDD.Application.DTO.Internal;

namespace CleanArchitecture.DDD.API.Models;

public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    /// <summary>
    /// The options name.
    /// </summary>
    public string? Name { get; }

    private readonly IValidator<TOptions> _validator;

    public FluentValidationOptions(string? name, IValidator<TOptions> validator)
    {
        Name = name;
        _validator = validator;
    }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        // Null name is used to configure all named options.
        if (Name != null && Name != name)
        {
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        var validationResult = _validator.Validate(options);

        if (validationResult.IsValid)
            return ValidateOptionsResult.Success;

        var errors = validationResult.Errors
            .GetValidationErrorByProperties(false)
            .ToFormattedJsonFailSafe();

        return ValidateOptionsResult.Fail(errors);
    }
}