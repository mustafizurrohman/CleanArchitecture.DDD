namespace CleanArchitecture.DDD.Infrastructure.Exceptions;

public class DatabaseNotReachableException : InfrastructureException
{
    public override string Message => "Invalid database connection string or database is not reachable ... ";
}