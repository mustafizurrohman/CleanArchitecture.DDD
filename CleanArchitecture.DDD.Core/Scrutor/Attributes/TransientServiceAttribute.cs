namespace CleanArchitecture.DDD.Core.Scrutor.Attributes;

/// <summary>
/// Attribute indicating that a Service must be used as a <b>Transient Service</b>
/// </summary>
public class TransientServiceAttribute : Attribute
{
    public TransientServiceAttribute()
    {
    }
}