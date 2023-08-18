using Hellang.Middleware.ProblemDetails;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;
using ValidationException = FluentValidation.ValidationException;

namespace CleanArchitecture.DDD.API.ExtensionMethods;

public static class ProblemDetailsOptionsExtensions
{
    public static void MapFluentValidationException(this ProblemDetailsOptions options) =>
        options.Map<ValidationException>((httpContext, validationException) =>
        {
            var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            var errors = validationException.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    error => error.Key,
                    error => error.Select(x => x.ErrorMessage).ToArray()
                );

            return factory.CreateValidationProblemDetails(httpContext, errors);
        });
}


