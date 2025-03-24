using DigitalDetox.Core.DTOs;
using DigitalDetox.Core.Entities;
using DigitalDetox.Core.Enums;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace DigitalDetox.Application.Servicies
{
    public class ChallengeService : IChallengeService
    {
        private readonly IChallengeRepos _Repos;
        public ChallengeService(IChallengeRepos Repos)
        {
            _Repos = Repos;
        }

        /// Note_1: DTOs are validated on the controler automatically using FluentValidation.
        /// Note_3: Services for mapping, Logging, Transactions SaveChanges, ...
        /// Note_3: In GET Let null returned and in controller validate it, also in POST validate null in the controller for using ActionResult
        /// Note_4: Throw an exception here, and handle it in the controller using TryCatch.
        public async Task<List<ChallengeGetDto>?> GetChallenges()
        {
            var challenges = _Repos.GetChallenges();
            if (challenges == null || !challenges.Any())
                throw new KeyNotFoundException("No challenges found!");

            var challengeDto = await challenges?.Select(ch => new ChallengeGetDto(ch)).ToListAsync()!;
            return challengeDto;
        }

        public async Task<ChallengeGetDto?> GetChallenge(int id)
        {
            var challenge = await _Repos.GetChallengeAsync(id);
            if (challenge == null)
                throw new KeyNotFoundException("Challenge Not Found!");

            var challengeDto = new ChallengeGetDto(challenge!);
            return challengeDto;
        }

        public async Task<List<ChallengeGetDto>?> GetStartedChallenges()
        {
            var challenges = _Repos.GetChallenges(ChallengeState.InProgress);
            var challengesDto = await challenges?.Select(ch => new ChallengeGetDto(ch)).ToListAsync()!;
            return challengesDto;
        }

        public async Task<List<ChallengeGetDto>?> GetAchivedChallenges()
        {
            var challenges = _Repos.GetChallenges(ChallengeState.Done);
            var challengesDto = await challenges?.Select(ch => new ChallengeGetDto(ch)).ToListAsync()!;
            return challengesDto;
        }

        public async Task<ChallengeGetDto?> DeleteChallengeAsync(int id)
        {
            var challenge = await _Repos.GetChallengeAsync(id);
            if (challenge == null)
                throw new KeyNotFoundException("Challenge not found.");

            _Repos.DeleteChallengeAsync(challenge);
            await _Repos.SaveAsync();

            ChallengeGetDto challengeDto = new ChallengeGetDto(challenge);
            return challengeDto;
        }

        public async Task AddChallengeAsync(ChallengePostDto newChallenge)
        {
            Challenge challenge = new Challenge(newChallenge);
            await _Repos.AddChallengeAsync(challenge);
            await _Repos.SaveAsync();
        }

        public async Task<ChallengeGetDto> UpdateChallengeAsync(int id, ChallengePostDto newChallenge)
        {
            Challenge? oldCha = await _Repos.GetChallengeAsync(id);
            if (oldCha == null)
                throw new KeyNotFoundException("Challenge not found.");

            oldCha.UpdateFromDto(newChallenge);

            _Repos.UpdateChallengeAsync(oldCha);
            await _Repos.SaveAsync();

            ChallengeGetDto challengeDto = new ChallengeGetDto(oldCha);
            return challengeDto;
        }

        public async Task<ChallengeGetDto?> CancelChallengeAsync(int id)
        {
            var challenge = await _Repos.GetChallengeAsync(id);
            if (challenge == null)
                throw new KeyNotFoundException("Challenge Not Found");

            if(challenge.State == ChallengeState.InProgress)
            {
                challenge.State = ChallengeState.Pending;
                challenge.StartDate = null;
                challenge.EndDate = null;

                await _Repos.SaveAsync();
                ChallengeGetDto chaDto = new ChallengeGetDto(challenge);
                return chaDto;
            }

            throw new Exception($"`{challenge.Title}` Challenge already not in progress");
        }
    }
}
