namespace CleanArchitecture.DDD.Application.ExtensionMethods;

public static class QueryableExtensions
{
    public static IQueryable<Doctor> SearchByName(this IQueryable<Doctor> query, Name name, bool and = false)
    {
        if (and)
        {
            query = query.Where(
                x => x.Name.Firstname.ToLower().Contains(name.Firstname.ToLower())
                     && x.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));
        }
        else
        {
            query = query.Where(
                x => x.Name.Firstname.ToLower().Contains(name.Firstname.ToLower())
                     || x.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));
        }

        return query;
    }
}