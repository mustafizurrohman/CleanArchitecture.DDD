namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.AssemblyScanning;


[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class InjectionOrderAttribute : Attribute
{
    public int Order { get; }

    public InjectionOrderAttribute(int order)
    {
        Order = order;
    }
}

