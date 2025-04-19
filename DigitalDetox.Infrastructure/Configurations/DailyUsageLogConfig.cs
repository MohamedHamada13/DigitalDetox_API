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
    public class DailyUsageLogConfig : IEntityTypeConfiguration<DailyUsageLog>
    {
        public void Configure(EntityTypeBuilder<DailyUsageLog> builder)
        {
            builder.HasKey(d => d.Id);

            //  Unique constraint to ensure one log per app per user per day
            builder.HasIndex(d => new { d.AppId, d.UserId, d.DailyLogDate })
                   .IsUnique();

            builder.Property(d => d.UsageTime)
                .IsRequired();
            builder.Property(d => d.DailyLogDate)
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
