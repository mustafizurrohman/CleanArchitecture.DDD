using FluentValidation.Results;
using ValidationException = FluentValidation.ValidationException;

namespace CleanArchitecture.DDD.API.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        object response = new
        {
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception)
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(response.ToFormattedJsonFailSafe());
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            // BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    
    private static IEnumerable<ValidationFailure> GetErrors(Exception exception)
    {
        IEnumerable<ValidationFailure> errors = Enumerable.Empty<ValidationFailure>();

        if (exception is ValidationException validationException)
            errors = validationException.Errors;
        
        return errors;
    }
}
