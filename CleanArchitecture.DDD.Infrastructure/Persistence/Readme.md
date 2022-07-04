- Open 'Package Manager Console'

## Default Project 
src/CleanArchitecture.DDD.API

## Adding a migration
dotnet ef migrations add <migration-name> --startup-project CleanArchitecture.DDD.API --project=CleanArchitecture.DDD.Infrastructure

## Removing a migraton
dotnet ef migrations remove --startup-project CleanArchitecture.DDD.API --project=CleanArchitecture.DDD.Infrastructure

# EFMigrationsHistory table can look like this

#Attention: EF Migrations does not always work smoothly in EF Core 7. Use with caution!

MigrationId	               ProductVersion
-----------------------------------------------
20220703144519_Reset	  7.0.0-preview.5.22302.2
20220703153455_SeedAddr	  7.0.0-preview.5.22302.2
