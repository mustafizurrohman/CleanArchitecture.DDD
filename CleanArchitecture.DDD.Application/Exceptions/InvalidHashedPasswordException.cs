namespace CleanArchitecture.DDD.Application.Exceptions;

public class InvalidHashedPasswordException : ApplicationException
{
    public override string Message => "Provided hash does not represent a valid hashed password";
}