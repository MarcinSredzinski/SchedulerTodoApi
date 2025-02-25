using Business.Library.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SchedulerTodo.DB;

public sealed class SoftDeleteInterceptor(TimeProvider _timeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return base.SavingChanges(eventData, result);
        }

        IEnumerable<EntityEntry<ISoftDeletable>> entries = eventData.Context.ChangeTracker
                                                            .Entries<ISoftDeletable>()
                                                            .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);

        foreach (EntityEntry<ISoftDeletable> softDeletable in entries)
        {
            softDeletable.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            softDeletable.Entity.IsDeleted = true;
            softDeletable.Entity.DeletedOnUtc = _timeProvider.GetUtcNow();
        }
        return base.SavingChanges(eventData, result);
    }
}