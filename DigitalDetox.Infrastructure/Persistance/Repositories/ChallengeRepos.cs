using DigitalDetox.Core.Context;
using DigitalDetox.Core.DTOs;
using DigitalDetox.Core.Entities;
using DigitalDetox.Core.Enums;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Persistance.Repositories
{
    public class ChallengeRepos : GenericRepos<Challenge>, IChallengeRepos
    {
        private readonly DegitalDbContext _ctx;
        public ChallengeRepos(DegitalDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Challenge>? GetChallenges(ChallengeState challengeState)
        {
            var challenges = _ctx.Challenges
                                .Where(ch => ch.State == challengeState);

            return challenges;
        }
    }
}
