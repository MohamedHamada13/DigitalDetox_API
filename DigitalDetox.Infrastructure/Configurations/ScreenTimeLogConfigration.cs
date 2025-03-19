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
    public class ScreenTimeLogConfigration : IEntityTypeConfiguration<ScreenTimeLog>
    {
        public void Configure(EntityTypeBuilder<ScreenTimeLog> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasOne(u => u.User)
                .WithMany(s => s.ScreenTimeLogs)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
