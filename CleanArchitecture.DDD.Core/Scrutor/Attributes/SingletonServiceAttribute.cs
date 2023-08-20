namespace CleanArchitecture.DDD.Core.Scrutor.Attributes;

/// <summary>
/// Attribute indicating that a Service must be used as a <b>Singleton Service</b>
/// </summary>
public class SingletonServiceAttribute : Attribute
{
    public SingletonServiceAttribute()
    {
    }
}