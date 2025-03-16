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
    internal class UsersChallengesConfigration : IEntityTypeConfiguration<UsersChallenges>
    {
        public void Configure(EntityTypeBuilder<UsersChallenges> builder)
        {
            builder.HasMany(u => u.Users)
                .WithOne(c => c.ChallengesForUser)
                .HasForeignKey(k => k.Id);
            builder.HasMany(u => u.Challenges)
                .WithOne(c=>c.ChallengesForUser)
                .HasForeignKey(k => k.Id);
            builder.HasKey(k => new { k.UserId, k.ChallengeId });

        }
    }
}
