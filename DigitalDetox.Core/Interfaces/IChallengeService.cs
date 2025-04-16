using DigitalDetox.Core.DTOs.ChallengeDto;
using DigitalDetox.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IChallengeService
    {
        Task<List<ChallengeGetDto>?> GetChallenges();
        Task<ChallengeGetDto?> GetChallenge(int id);
        Task<List<ChallengeGetDto>?> GetStartedChallenges();
        Task<List<ChallengeGetDto>?> GetAchivedChallenges();
        Task AddChallengeAsync(ChallengePostDto cha);
        Task<ChallengeGetDto?> CancelChallengeAsync(int id);
        Task<ChallengeGetDto> UpdateChallengeAsync(int id, ChallengePostDto newChallenge);
        Task<ChallengeGetDto?> DeleteChallengeAsync(int id);
    }
}
