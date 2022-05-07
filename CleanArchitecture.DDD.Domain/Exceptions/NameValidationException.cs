using System.Runtime.Serialization;
using FluentValidation.Results;

namespace CleanArchitecture.DDD.Domain.Exceptions;

internal class NameValidationException : DomainValidationException
{
    public override string Message => "Invalid name";

    public NameValidationException(string message) : base(message)
    {
    }

    public NameValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message, errors)
    {
    }

    public NameValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage) : base(message, errors, appendDefaultMessage)
    {
    }

    public NameValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
    {
    }

    public NameValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}