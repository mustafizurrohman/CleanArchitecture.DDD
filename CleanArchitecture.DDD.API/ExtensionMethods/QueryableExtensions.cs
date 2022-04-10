namespace CleanArchitecture.DDD.API.ExtensionMethods
{
    public static class QueryableExtensions
    {
        public static IQueryable<Doctor> SearchByName(this IQueryable<Doctor> query, Name name, bool and = false)
        {        
            query = query.AsNoTracking();

            if (and)
            {
                query = query.Where(
                    x => x.Name.Firstname.ToLower().Contains(name.Firstname.ToLower())
                      && x.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));
            } else
            {
                query = query.Where(
                    x => x.Name.Firstname.ToLower().Contains(name.Firstname.ToLower())
                      || x.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));
            }

            return query;
        }

        /*
        public static IQueryable<Doctor> Search<TSource>(this IQueryable<Doctor> query, Expression<Func<Doctor, bool>> condition, bool and = false)
        {
            query = query.AsNoTracking();

            if (and)
            {
                query = query.Where(
                    x => x.Name.Firstname.ToLower().Contains(condition.Name. .ToLower())
                      && x.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));
            } else
            {
                query = query.Where(
                    x => x.Name.Firstname.ToLower().Contains(name.Firstname.ToLower())
                      || x.Name.Lastname.ToLower().Contains(name.Lastname.ToLower()));
            }

            return query;
        } */
    }
}
