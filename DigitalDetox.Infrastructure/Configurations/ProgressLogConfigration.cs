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
    public class ProgressLogConfigration : IEntityTypeConfiguration<ProgressLog>
    {
        public void Configure(EntityTypeBuilder<ProgressLog> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.UserId)
                .HasDefaultValue(null);
        }
    }
}
