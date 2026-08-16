using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Infrastructure.Extensions
{
    /// <summary>
    /// EF Core model builder conventions for shared fields
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Apply unified audit field configurations for all BaseEntity derived types
        /// </summary>
        public static void ApplyAuditFieldConventions(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                
                if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                    continue;

                var builder = modelBuilder.Entity(entityType.ClrType);               
                builder.Property(nameof(BaseEntity.CreatedOnUtc)) .HasColumnType("datetime2(6)").HasDefaultValueSql("GETUTCDATE()").IsRequired();
                builder.Property(nameof(BaseEntity.UpdatedOnUtc)).HasColumnType("datetime2(6)").HasDefaultValueSql("GETUTCDATE()").IsRequired();
            }
        }
    }
}
