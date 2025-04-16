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
    public class ScreenTimeLogConfigration : IEntityTypeConfiguration<ScreenTimeLog>
    {
        public void Configure(EntityTypeBuilder<ScreenTimeLog> builder)
        {
            builder.HasKey(s => s.Id);

            // RS with AppUser
            builder.HasOne(s => s.User)
                .WithMany(u => u.ScreenTimeLogs)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
