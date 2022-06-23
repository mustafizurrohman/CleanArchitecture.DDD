namespace CleanArchitecture.DDD.Infrastructure.Exceptions;

public class InfrastructureException : Exception
{
    public InfrastructureException(string exceptionMessage, Exception innerException)
        : base(exceptionMessage, innerException)
    {

    }

    public InfrastructureException(string exceptionMessage) : base(exceptionMessage)
    {

    }

    public InfrastructureException()
    {

    }
}