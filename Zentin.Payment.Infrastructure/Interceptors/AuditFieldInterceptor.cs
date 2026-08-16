
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Infrastructure.Interceptors
{
    /// <summary>
    /// automatically fills audit fields (CreatedOnUtc / UpdatedOnUtc) for entities that inherit from BaseEntity 
    /// during SaveChanges and SaveChangesAsync operations.
    /// </summary>
    public sealed class AuditFieldInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void UpdateAuditFields(DbContext? context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOnUtc = now;
                        entry.Entity.UpdatedOnUtc = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedOnUtc = now;
                        break;
                }
            }
        }
    }
}
