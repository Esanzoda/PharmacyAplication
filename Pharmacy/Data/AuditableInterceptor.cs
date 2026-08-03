using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pharmacy.Models.Domain;

namespace Pharmacy.Data;

public class AuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is not null)
        {
            var entityEntries = eventData.Context.ChangeTracker.Entries<BaseEntity>();

            foreach (var entityEntry in entityEntries)
            {
                if (entityEntry.State is EntityState.Added)
                {
                    entityEntry.Entity.CreatedAt = DateTime.UtcNow;
                    entityEntry.Entity.UpdateAt = DateTime.UtcNow;
                }

                if (entityEntry.State is EntityState.Modified)
                {
                    entityEntry.Entity.UpdateAt = DateTime.UtcNow;
                }

                if (entityEntry.State is EntityState.Deleted)
                {
                    entityEntry.Entity.IsDeleted = true;
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}