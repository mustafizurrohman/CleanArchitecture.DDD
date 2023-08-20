using System.Collections.Immutable;
using System.Net;
using Microsoft.AspNetCore.Http;
using ValidationException = FluentValidation.ValidationException;

namespace CleanArchitecture.DDD.Application.MediatR.PipelineBehaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>>? _validators;
    
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>>? validators)
    {
        _validators = validators;
        Console.WriteLine("Initialized validation behaviour");
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators is null) 
            return await next();
        
        var context = new ValidationContext<TRequest>(request);

        var errors = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .ToImmutableList();

        if (errors.Any())
            throw new ValidationException(errors);
        
        return await next();
    }
}
