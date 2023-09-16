using System.Collections.Immutable;
using System.Linq.Expressions;
using CleanArchitecture.DDD.Core.ExtensionMethods;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
// ReSharper disable UnusedMethodReturnValue.Global

namespace CleanArchitecture.DDD.Infrastructure.Persistence.ExtensionMethods;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ConfigureSoftDelete(this ModelBuilder modelBuilder)
    {
        // Set default value of SoftDeletedProperty in all entities
        var allSoftDeletedProperties = modelBuilder.GetAllEntityTypes()
            .SelectMany(type => type.GetProperties())
            .Where(p => p.Name == nameof(BaseEntity.SoftDeleted))
            .ToList();

        foreach (var prop in allSoftDeletedProperties)
        {
            prop.SetDefaultValue(false);
        }

        return modelBuilder;

    }

    public static ModelBuilder ConfigureGlobalFilters(this ModelBuilder modelBuilder)
    {
        Expression<Func<BaseEntity, bool>> notSoftDeletedFilterExpr = bm => !bm.SoftDeleted;

        modelBuilder.GetAllEntityTypes()
            .Where(et => et.ClrType.IsAssignableTo(typeof(BaseEntity)))
            .ForEach(entityType =>
            {
                // Modify expression to handle correct child type
                var parameter = Expression.Parameter(entityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(notSoftDeletedFilterExpr.Parameters[0], parameter, notSoftDeletedFilterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

                // Set query filter
                entityType.SetQueryFilter(lambdaExpression);
            });

        return modelBuilder;

    }

    private static IReadOnlyList<IMutableEntityType> GetAllEntityTypes(this ModelBuilder modelBuilder)
    {
        return modelBuilder.Model
            .GetEntityTypes()
            .ToImmutableList();
    }
}

