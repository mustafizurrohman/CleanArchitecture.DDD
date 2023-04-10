namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.AssemblyScanning;

[InjectionOrder(3)]
public class ThreeTestService : ITestService
{
    public string HelloWorld()
    {
        return "Hello World from Test Service Three (3)";
    }
}