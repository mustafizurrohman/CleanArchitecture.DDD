namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal static class DemoDataExtensions
{
    public static DemoData GetCachedVersion(this DemoData demoData)
    {
        return new DemoData()
        {
            Cached = true,
            CreatedDateTime = demoData.CreatedDateTime,
            Firstname = demoData.Firstname,
            Lastname = demoData.Lastname
        };
    }
}