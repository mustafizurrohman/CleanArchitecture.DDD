using System.Runtime.Serialization;
using FluentValidation.Results;

namespace CleanArchitecture.DDD.Domain.Exceptions;

/// <inheritdoc />
public class DomainValidationException : ValidationException
{
    protected DomainValidationException(string message) 
        : base(message)
    {
    }

    protected DomainValidationException(string message, IEnumerable<ValidationFailure> errors) 
        : base(message, errors)
    {
    }

    protected DomainValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage) 
        : base(message, errors, appendDefaultMessage)
    {
    }

    protected DomainValidationException(IEnumerable<ValidationFailure> errors) 
        : base(errors)
    {
    }

    protected DomainValidationException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
}