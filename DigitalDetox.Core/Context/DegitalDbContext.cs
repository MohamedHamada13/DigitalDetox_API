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
    internal class DegitalDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // connection string

            //optionsBuilder.UseSqlServer("Server=.;Database=DigitalDetoxAPI;Trusted_Connection=true;TrustServerCertificate=true;");

                   }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<User>  Users{ get; set; }
        public DbSet<ScreenTimeLog>  ScreenTimeLogs { get; set; }
        public DbSet<ProgressLog>   ProgressLogs{ get; set; }
        public DbSet<DetoxPlan>  DetoxPlans { get; set; }
        public DbSet<Challenge>  Challenges { get; set; }
    }
}
