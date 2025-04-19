using DigitalDetox.Core.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Context
{
    public class DegitalDbContext : IdentityDbContext<AppUser>
    {
        public DegitalDbContext(DbContextOptions<DegitalDbContext> options) : base(options) { }

        public DbSet<DailyUsageLog> DailyUsageLogs { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<ProgressLog> ProgressLogs{ get; set; }
        public DbSet<DetoxPlan> DetoxPlans { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<UserStoreTemporary> UserStoreTemporary { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
