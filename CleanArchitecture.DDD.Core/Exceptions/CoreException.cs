namespace CleanArchitecture.DDD.Core.Exceptions;

public class CoreException : Exception
{
    public CoreException(string exceptionMessage, Exception innerException)
        : base(exceptionMessage, innerException)
    {

    }

    public CoreException(string exceptionMessage) : base(exceptionMessage)
    {

    }

    public CoreException()
    {

    }
}