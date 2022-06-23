namespace CleanArchitecture.DDD.Application.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string exceptionMessage, Exception innerException)
        : base(exceptionMessage, innerException)
    {

    }

    public ApplicationException(string exceptionMessage) : base(exceptionMessage)
    {

    }

    public ApplicationException()
    {

    }
}