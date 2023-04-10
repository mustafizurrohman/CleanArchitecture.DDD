using CleanArchitecture.DDD.API.Models;

namespace CleanArchitecture.DDD.API.ExtensionMethods;

public static class OptionsBuilderFluentValidationExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) 
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>
            (service => new FluentValidationOptions<TOptions>(optionsBuilder.Name, service.GetRequiredService<IValidator<TOptions>>()));

        return optionsBuilder;
    }
}