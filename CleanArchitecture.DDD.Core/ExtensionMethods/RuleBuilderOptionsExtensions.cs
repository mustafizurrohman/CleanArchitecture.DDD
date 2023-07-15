using CleanArchitecture.DDD.Core.ValueObjects;
using CSharpFunctionalExtensions;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class RuleBuilderOptionsExtensions
{
    /// <summary>
    /// Checks if a name is valid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> MustBeValidName<T>(this IRuleBuilder<T, string> rule, int maxLength = 30)
    {
        return rule
            .MustNotBeNullOrEmpty()
            .MaximumLength(maxLength)
                .WithMessage("'{PropertyName}' cannot have more than 30 characters")
            .NotContainNumbersOrSpecialCharacters()
            .MustNotStartOrEndWithWhiteSpace()
            .MustNotContainConsecutiveSpaces();
    }

    /// <summary>
    /// Check if a property is Null or Empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="rule"></param>
    /// <returns></returns>
    // ReSharper disable once MemberCanBePrivate.Global
    public static IRuleBuilderOptions<T, TProperty> MustNotBeNullOrEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> rule)
    {
        return rule
            .SetValidator(new NotEmptyValidator<T, TProperty>())
                .WithMessage("'{PropertyName}' must not be empty.")
            .SetValidator(new NotNullValidator<T, TProperty>())
                .WithMessage("'{PropertyName}' must not be Null.");
    }


    /// <summary>
    /// Checks if a property starts with a white space
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> NotStartWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(m => m != null && !m.StartsWith(" "))
            .WithMessage("'{PropertyName}' must not start with whitespace.");
    }

    /// <summary>
    /// Checks if a property ends with a white space
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> NotEndWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(m => m != null && !m.EndsWith(" "))
            .WithMessage("'{PropertyName}' must not end with whitespace.");
    }

    /// <summary>
    /// Check if a property starts or ends with a white space 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> MustNotStartOrEndWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotStartWithWhiteSpace()
            .NotEndWithWhiteSpace();
    }

    /// <summary>
    /// Checks if a property contains a digit or special character
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> NotContainNumbersOrSpecialCharacters<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(name => !name.Any(char.IsDigit))
            .WithMessage("'{PropertyName}' must not contain numbers");
    }

    /// <summary>
    /// Checks if a property contains consecutive spaces
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    private static IRuleBuilderOptions<T, string> MustNotContainConsecutiveSpaces<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(name => !name.Contains("  "))
            .WithMessage("'{PropertyName}' must not contain more than 1 consecutive spaces");

    }

    public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
        this IRuleBuilder<T, string> ruleBuilder,
        Func<string, Result<TValueObject, Error>> factoryMethod)
        where TValueObject : ValueObject
    {
        return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(value);

            if (result.IsFailure)
            {
                context.AddFailure(result.Error.Serialize());
            }
        });
    }

}