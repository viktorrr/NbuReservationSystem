namespace NbuReservationSystem.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using Common.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private static readonly Func<DbEntityEntry, bool> AuditFilter =
            e => e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified));

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public IDbSet<Organizer> Organizers { get; set; }

        public IDbSet<Reservation> Reservations { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in this.ChangeTracker.Entries().Where(AuditFilter))
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
