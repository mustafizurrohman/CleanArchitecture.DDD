namespace CleanArchitecture.DDD.Core.Scrutor.Attributes;

/// <summary>
/// Attribute indicating that a Service must be used as a <b>Scoped Service</b>
/// </summary>
[UsedImplicitly]
public class ScopedServiceAttribute : Attribute
{
    public ScopedServiceAttribute()
    {
    }
}