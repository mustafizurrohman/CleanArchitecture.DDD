namespace CleanArchitecture.DDD.API.ExtensionMethods
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Migrates the database.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        public static void MigrateDatabase(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope? serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScope is null) 
                return;
            
            using var context = serviceScope.ServiceProvider.GetService<DomainDbContext>();
            try
            {
                Log.Information("Starting database migration ...");
                context?.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("An exception occurred during migrating database ...");
                Log.Error(ex.ToString());
            }
            finally
            {
                Log.Information("Database Migration completed ...");
            }

        }
    }
}