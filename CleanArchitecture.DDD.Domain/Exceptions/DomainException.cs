namespace CleanArchitecture.DDD.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string exceptionMessage, Exception innerException)
        : base(exceptionMessage, innerException)
    {

    }

    public DomainException(string exceptionMessage) : base(exceptionMessage)
    {

    }

    public DomainException()
    {

    }
}