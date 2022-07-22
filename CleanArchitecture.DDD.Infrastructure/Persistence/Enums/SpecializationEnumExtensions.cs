using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

public static class SpecializationEnumExtensions
{

    private static readonly Dictionary<Enum, string> EnumStringValues = new();
    public static string ToStringCached(this Specialization specialization)
    {
        if (EnumStringValues.TryGetValue(specialization, out var textValue))
            return textValue;
        
        textValue = specialization.ToString().CamelCaseToSentence();
        EnumStringValues[specialization] = textValue;
        return textValue;
    }

    public static Specialization ToSpecialization(this string inputString)
    {
        return Enum.TryParse<Specialization>(inputString.Replace(" ", string.Empty), true, out var parsedSpecialization)
            ? parsedSpecialization
            : Specialization.Unknown;
    }

    public static Specialization GetRandomSpecialization()
    {
        return Enum.GetValues(typeof(Specialization))
            .ToListDynamic()
            .Select(specialization => (Specialization)specialization)
            .Where(specialization => specialization != Specialization.Unknown)
            .MinBy(_ => Guid.NewGuid());
    }
}