using DigitalDetox.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Confegrations
{
    public class DetoxPlanConfigration : IEntityTypeConfiguration<DetoxPlan>
    {
        public void Configure(EntityTypeBuilder<DetoxPlan> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValue(DateTime.Now);

            // RS with AppUser
            builder.HasOne(d => d.User)
                .WithMany(u => u.DetoxPlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
