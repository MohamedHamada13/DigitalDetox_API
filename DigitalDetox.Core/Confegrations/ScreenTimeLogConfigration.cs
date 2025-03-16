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
    internal class ScreenTimeLogConfigration : IEntityTypeConfiguration<ScreenTimeLog>
    {
        public void Configure(EntityTypeBuilder<ScreenTimeLog> builder)
        {
            builder.HasOne(u => u.User)
                .WithMany(s => s.ScreenTimeLogs)
                .HasForeignKey(k => k.UserId);

        }
    }
}
