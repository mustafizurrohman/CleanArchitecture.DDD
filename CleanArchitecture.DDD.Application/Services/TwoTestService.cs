namespace CleanArchitecture.DDD.Application.Services;

public class TwoTestService : ITestService
{
    public string HelloWorld()
    {
        return "Hello World from Test Service Two (2)";
    }
}