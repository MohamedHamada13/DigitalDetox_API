using DigitalDetox.Core.DTOs;
using DigitalDetox.Core.Entities;
using DigitalDetox.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IChallengeRepos : IGenericRepos<Challenge>
    {
        IQueryable<Challenge>? GetChallenges(ChallengeState challengeState);
    }
}
