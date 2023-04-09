namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class PropertyNameAttemptedValue
{
    public string Name { get; }
    public object? AttemptedValue { get; }


    public PropertyNameAttemptedValue(string name, object? attemptedValue)
    {
        Name = name;
        AttemptedValue = attemptedValue;
    }
}