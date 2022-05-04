using FluentValidation.Results;

namespace CleanArchitecture.DDD.Domain.Extensions
{
    public static class FluentValidationResultExtensions
    {
        public static string ToErrorString(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
                return string.Empty;

            return validationResult.Errors
                .Select((e, index) => (index + 1) + "- " + e)
                .Aggregate((e1, e2) => e1 + Environment.NewLine + e2);
        }
    }
}