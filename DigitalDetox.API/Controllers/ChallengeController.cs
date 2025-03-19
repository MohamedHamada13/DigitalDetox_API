using DigitalDetox.Core.DTOs;
using DigitalDetox.Core.Context;
using DigitalDetox.Core.Enums;
using DigitalDetox.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalDetox.Application.Validators;

namespace DigitalDetox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly DegitalDbContext _context;
        public ChallengeController(DegitalDbContext context)
        {
            _context = context;
        }

        #region GET Actions
        [HttpGet] // Apply Pagination
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ChallengeGetDto>>> GetChallenges()
        {
            var challenges = await _context.Challenges.ToListAsync();

            if (challenges == null || challenges.Count <= 0)
                return NotFound();

            List<ChallengeGetDto> challengesDto = challenges.Select(ch => new ChallengeGetDto(ch)).ToList();

            return Ok(challengesDto);
        }

        [HttpGet("started")] // Apply Pagination
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ChallengeGetStartedDto>>> GetStartedChallenges()
        {
            List<Challenge> challenges = await _context.Challenges
                .Where(ch => ch.State == ChallengeState.InProgress)
                .ToListAsync();

            if (challenges == null || challenges.Count <= 0)
                return NoContent();

            List<ChallengeGetStartedDto> challengesDto = challenges.Select(ch => new ChallengeGetStartedDto(ch)).ToList();

            return Ok(challengesDto);
        }

        [HttpGet("done")] // Apply Pagination
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ChallengeGetStartedDto>>> GetAchivedChallenges()
        {
            List<Challenge> challenges = await _context.Challenges
                .Where(ch => ch.State == ChallengeState.Done)
                .ToListAsync();

            if (challenges == null || challenges.Count <= 0)
                return NoContent();

            List<ChallengeGetStartedDto> challengesDto = challenges.Select(ch => new ChallengeGetStartedDto(ch)).ToList();

            return Ok(challengesDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ChallengeGetDto>> GetChallengeById(int id)
        {
            if (id <= 0) 
                return BadRequest();

            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
                return NotFound();

            ChallengeGetDto challengeDto = new ChallengeGetDto(challenge);
            return Ok(challengeDto);
        }
        #endregion

        #region POST Actions
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateChallenge([FromBody] ChallengePostDto newChallenge)
        {
            #region Manual Validation before using automatic validation from assemply using FluentValidation.AspNetCore tool
            /*
            var challengePostValidator = new ChallengePostDtoValidator();
            var validationResult = challengePostValidator.Validate(newChallenge);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            */
            #endregion
            // Creation Process
            var challenge = new Challenge(newChallenge); // Mapping
            await _context.Challenges.AddAsync(challenge);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetChallengeById), new { id = challenge.Id }, new ChallengeGetDto(challenge));
        }

        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateChallenges([FromBody] List<ChallengePostDto> newChallenges)
        {
            if(!newChallenges.Any() || newChallenges == null)
                return BadRequest("Challenge list cannot be empty.");

            List<Challenge> challenges = newChallenges.Select(ch => new Challenge(ch)).ToList();
            
            await _context.Challenges.AddRangeAsync(challenges);
            await _context.SaveChangesAsync();

            return Created("", newChallenges);
        }
        #endregion

        #region PUT Action
        [HttpPut("{id}/Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> StartChallenge(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Id");

            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null)
                return NotFound();

            if (challenge.State != ChallengeState.Pending)
                return BadRequest($"Challenge[{challenge.Title}] has already been started or completed.");

            // method to initialize StartDate & EndDate, and set ChallengeState as InProgres
            challenge.StartChallenge();
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Challenge started", StartDate = challenge.StartDate, EndDate = challenge.EndDate });
        }

        [HttpPut("{id}/MakeDone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> MakeChallengeDone(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null)
                return NotFound();

            if (challenge.State == ChallengeState.Pending) 
                return BadRequest($"Challenge[{challenge.Title}] not started yet.");


            challenge.State = ChallengeState.Done;
            ChallengeGetDto challengeDto = new ChallengeGetDto(challenge);

            await _context.SaveChangesAsync();
            return Ok(challengeDto);
        }

        [HttpPut("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CancelChallenge(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null)
                return NotFound();

            if (challenge.State == ChallengeState.InProgress)
            {
                challenge.State = ChallengeState.Pending;
                await _context.SaveChangesAsync();
                return Ok($"'{challenge.Title}' challenge has canceled");
            }

            return BadRequest($"'{challenge.Title}' already not in progress");
        }
        #endregion

        #region DELETE Actions
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteChallenge(int id)
        {
            if (id <= 0)
                return BadRequest();

            Challenge? challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null) 
                return NotFound();

            _context.Challenges.Remove(challenge);
            await _context.SaveChangesAsync();

            return Ok($"`{challenge.Title}` challenge has been deleted");
        }
        #endregion
    }
}
