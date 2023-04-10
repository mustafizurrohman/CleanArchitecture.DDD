namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.AssemblyScanning;

[InjectionOrder(1)]
public class OneTestService : ITestService
{
    public string HelloWorld()
    {
        return "Hello World from Test Service One (1)";
    }
}