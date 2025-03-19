using DigitalDetox.Core.Entities;
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

            builder.Property(p => p.Title).IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasDefaultValue(DateTime.Now);

            builder.HasOne(u => u.User)
                .WithMany(d => d.DetoxPlans)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
