namespace CleanArchitecture.DDD.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectionOrderAttribute : Attribute
{
    public int Order { get; }

    public InjectionOrderAttribute(int order)
    {
        Order = order;
    }
}


