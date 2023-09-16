﻿using System.Collections.Immutable;
using CleanArchitecture.DDD.Core.Models;
using CleanArchitecture.DDD.Infrastructure.Exceptions;
using CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using DatabaseContext = Microsoft.EntityFrameworkCore.DbContext;
using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;

public class DomainDbContext : DatabaseContext
{
    public DomainDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    #region -- Entities --

    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Patient> Patients { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {      
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var allEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .ToImmutableList();

        #region -- Soft Deletion Configuration --

        // Set default value of SoftDeletedProperty in all entities
        var allSoftDeletedProperties = allEntityTypes
            .SelectMany(type => type.GetProperties())
            .Where(p => p.Name == nameof(BaseEntity.SoftDeleted))
            .ToList();

        foreach (var prop in allSoftDeletedProperties)
        {
            prop.SetDefaultValue(false);
        }

        #endregion

        #region -- Global Query Filter Configuration --

        Expression<Func<BaseEntity, bool>> notSoftDeletedFilterExpr = bm => !bm.SoftDeleted;

        allEntityTypes
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

        #endregion
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditingData();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        SetAuditingData();
        return base.SaveChanges();
    }

    /// <summary>
    /// Warning: This can degrade the performance
    /// </summary>
    private void SetAuditingData()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is {Entity: BaseEntity, State: EntityState.Added or EntityState.Modified});

        var now = DateTime.Now;

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
                ((BaseEntity)entityEntry.Entity).CreatedOn = now;
            else
                ((BaseEntity)entityEntry.Entity).UpdatedOn = now;
        }
                       
    }
}