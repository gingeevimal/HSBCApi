using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class HSBCContext : DbContext
    {
        public HSBCContext(DbContextOptions<HSBCContext> options) : base(options)
        {
        }

        public DbSet<Order> orders { get; set; }
        public DbSet<CAdocument> cadocuments { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    //case EntityState.Added:
                    //    entry.Entity.CreatedDate = DateTime.Now;
                    //    entry.Entity.CreatedBy = "swn";
                    //    break;
                    //case EntityState.Modified:
                    //    entry.Entity.LastModifiedDate = DateTime.Now;
                    //    entry.Entity.LastModifiedBy = "swn";
                    //    break;
                    case EntityState.Added:
                        entry.Entity.createddate = DateTime.Now;
                        entry.Entity.createdby = "swn";
                        break;
                    case EntityState.Modified:
                        entry.Entity.lastmodifieddate = DateTime.Now;
                        entry.Entity.lastmodifiedby = "swn";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
