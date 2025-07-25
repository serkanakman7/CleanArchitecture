using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Repositories.Interceptors
{
    public class AuditDbContextInterceptor : SaveChangesInterceptor
    {
        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> _behaviors = new()
        {
            {EntityState.Added,AddBehavior },
            {EntityState.Modified,ModifiedBehavior }
        };

        private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.Created = DateTime.Now;
            context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;
        }

        private static void ModifiedBehavior(DbContext context, IAuditEntity auditEntity)
        {
            context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
            auditEntity.Updated = DateTime.Now;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            foreach(var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
            {
                if(entityEntry.Entity is not IAuditEntity auditEntity)
                {
                    continue;
                }

                if(entityEntry.State is not EntityState.Added or EntityState.Modified)
                {
                    continue;
                }

                _behaviors[entityEntry.State](eventData.Context, auditEntity);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        //        switch (entityEntry.State)
        //        {
        //            case EntityState.Added:

        //                auditEntity.Created = DateTime.Now;
        //                eventData.Context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;
        //                break;
        //            case EntityState.Modified:

        //                eventData.Context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
        //                auditEntity.Updated = DateTime.Now;
        //                break;
        //        }
}
}
