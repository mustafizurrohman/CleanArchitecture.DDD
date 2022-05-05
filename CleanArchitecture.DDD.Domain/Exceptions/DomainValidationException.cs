using System.Runtime.Serialization;
using FluentValidation.Results;

namespace CleanArchitecture.DDD.Domain.Exceptions;

public class DomainValidationException : ValidationException
{
    public DomainValidationException(string message) : base(message)
    {
    }

    public DomainValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message, errors)
    {
    }

    public DomainValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage) : base(message, errors, appendDefaultMessage)
    {
    }

    public DomainValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
    {
    }

    public DomainValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}