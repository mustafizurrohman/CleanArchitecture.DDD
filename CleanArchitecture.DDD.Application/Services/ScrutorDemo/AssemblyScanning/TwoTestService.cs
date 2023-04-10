namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.AssemblyScanning;

[InjectionOrder(2)]
public class TwoTestService : ITestService
{
    public string HelloWorld()
    {
        return "Hello World from Test Service Two (2)";
    }
}