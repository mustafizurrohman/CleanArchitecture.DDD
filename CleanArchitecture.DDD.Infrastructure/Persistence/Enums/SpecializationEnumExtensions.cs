using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.Enums;

public static class SpecializationEnumExtensions
{
    //TODO: Optimize this!
    public static string ToReadableString(this Specialization specialization)
    {
        //return specialization switch
        //{
        //    Specialization.GeneralPractice => "General Practice",
        //    _ => "Unknown"
        //};

        return specialization.ToString().CamelCaseToSentence();
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