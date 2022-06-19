namespace CleanArchitecture.DDD.Application.Services;

public class OneTestService : ITestService
{
    public string HelloWorld()
    {
        return "Hello World from Test Service One (1)";
    }
}