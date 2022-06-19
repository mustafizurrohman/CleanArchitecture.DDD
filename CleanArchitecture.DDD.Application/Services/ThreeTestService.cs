namespace CleanArchitecture.DDD.Application.Services;

public class ThreeTestService : ITestService
{
    public string HelloWorld()
    {
        return "Hello World from Test Service Three (3)";
    }
}