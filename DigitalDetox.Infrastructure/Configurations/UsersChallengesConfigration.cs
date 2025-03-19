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
    public class UsersChallengesConfigration : IEntityTypeConfiguration<UsersChallenges>
    {
        public void Configure(EntityTypeBuilder<UsersChallenges> builder)
        {
            builder.HasKey(uc => new { uc.UserId, uc.ChallengeId }); // Composite PK

            // User & UserChallenge Relationship
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.UserChallenges)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Challenge & UserChallenges Relationship
            builder.HasOne(uc => uc.Challenge)
                .WithMany(c => c.UserChallenges)
                .HasForeignKey(uc => uc.ChallengeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
