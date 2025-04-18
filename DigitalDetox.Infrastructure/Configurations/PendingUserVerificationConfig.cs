using DigitalDetox.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Configurations
{
    public class PendingUserVerificationConfig : IEntityTypeConfiguration<UserStoreTemporary>
    {
        public void Configure(EntityTypeBuilder<UserStoreTemporary> builder)
        {
            builder.HasKey(k => k.Email);
        }
    }
}
