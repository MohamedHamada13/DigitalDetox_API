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
    internal class ProgressLogConfigration : IEntityTypeConfiguration<ProgressLog>
    {
        public void Configure(EntityTypeBuilder<ProgressLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(u => u.User)
                .WithMany(p => p.ProgressLogs)
                .HasForeignKey(f => f.UserId);
        }
    }
}
