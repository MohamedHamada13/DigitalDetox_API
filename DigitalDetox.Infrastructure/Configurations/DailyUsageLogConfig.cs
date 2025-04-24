using DigitalDetox.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Confegrations
{
    public class DailyUsageLogConfig : IEntityTypeConfiguration<UserUsageLog>
    {
        public void Configure(EntityTypeBuilder<UserUsageLog> builder)
        {
            builder.HasKey(d => d.Id);

            //  Unique constraint to ensure one log per app per user per day
            builder.HasIndex(d => new { d.AppId, d.UserId, d.DailyLogDate })
                   .IsUnique();

            builder.Property(d => d.UsageTime)
                .IsRequired();
            builder.Property(d => d.DailyLogDate)
                .IsRequired();

            // This ensures EF knows how to map DateOnly to a SQL column, because SQL only understands DateTime or date.
            builder.Property(d => d.DailyLogDate)
                   .HasConversion(
                       v => v.ToDateTime(TimeOnly.MinValue),   // Convert DateOnly -> DateTime for the DB
                       v => DateOnly.FromDateTime(v)           // Convert DateTime -> DateOnly from the DB
                   )
                   .HasColumnType("date") // Optional: use SQL Server "date" type (no time component)
                   .IsRequired();


            // RS with AppUser
            builder.HasOne(d => d.User)
                .WithMany(u => u.DailyUsageLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // RS with App
            builder.HasOne(d => d.App)
                .WithMany(a => a.DailyLogs)
                .HasForeignKey(d => d.AppId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
