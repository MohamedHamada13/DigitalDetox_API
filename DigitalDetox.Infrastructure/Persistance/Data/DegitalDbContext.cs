using DigitalDetox.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Context
{
    public class DegitalDbContext : DbContext
    {
        public DegitalDbContext(DbContextOptions<DegitalDbContext> options) : base(options) { }

        public DbSet<User> Users{ get; set; }
        public DbSet<ScreenTimeLog> ScreenTimeLogs { get; set; }
        public DbSet<ProgressLog> ProgressLogs{ get; set; }
        public DbSet<DetoxPlan> DetoxPlans { get; set; }
        public DbSet<Challenge> Challenges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgressLog>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.ProgressLogs)
                .HasForeignKey(pl => pl.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
