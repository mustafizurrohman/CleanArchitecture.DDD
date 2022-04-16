## Default Project 
src/CleanArchitecture.DDD.API

## Adding a migration
dotnet ef migrations add <migration-name> --startup-project CleanArchitecture.DDD.API --project=CleanArchitecture.DDD.Infrastructure

## Removing a migraton
dotnet ef migrations remove --startup-project CleanArchitecture.DDD.API --project=CleanArchitecture.DDD.Infrastructure