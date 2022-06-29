using CleanArchitecture.DDD.Core.ExtensionMethods;
using CleanArchitecture.DDD.Infrastructure.Migrations;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

public static class SpecializationEnumExtensions
{
    public static string ToReadableString(this Specialization specialization)
    {
        return specialization.ToString().CamelCaseToSentence();
    }

    public static Specialization ToSpecialization(this string inputString)
{
        return Enum.TryParse<Specialization>(inputString.Replace(" ", string.Empty), true, out var parsedSpecialization)
            ? parsedSpecialization
            : Specialization.Unknown;
    }
}