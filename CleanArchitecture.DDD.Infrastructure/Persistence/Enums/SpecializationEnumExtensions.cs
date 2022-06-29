using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

public static class SpecializationEnumExtensions
{
    public static string ToReadableString(this Specialization specialization)
    {
        return specialization.ToString().CamelCaseToSentence();
    }
}