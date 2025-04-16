using DigitalDetox.Core.Context;
using DigitalDetox.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalDetox.Application.Validators;
using DigitalDetox.Core.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.DTOs.ChallengeDto;

namespace DigitalDetox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly DegitalDbContext _context;
        private readonly IChallengeService _challengeService;
        public ChallengeController(DegitalDbContext context, IChallengeService challengeService)
        {
            _context = context;
            _challengeService = challengeService;
        }

        #region GET Actions
        [HttpGet] // Apply Pagination
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ChallengeGetDto>?>> GetChallenges()
        {
            var challengesDto = await _challengeService.GetChallenges();

            if (challengesDto == null || challengesDto.Count <= 0)
                return NoContent();

            return Ok(challengesDto);
        }

        [HttpGet("started")] // Apply Pagination
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ChallengeGetDto>?>> GetStartedChallenges()
        {
            var challengesDto = await _challengeService.GetStartedChallenges();
            if (challengesDto == null || challengesDto.Count <= 0)
                return NoContent();

            return Ok(challengesDto);
        }

        [HttpGet("done")] // Apply Pagination
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ChallengeGetDto>>> GetAchivedChallenges()
        {
            try
            {
                var challengesDto = await _challengeService.GetAchivedChallenges();
                return Ok(challengesDto);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ChallengeGetDto?>> GetChallengeByIdAsync(int id)
        {
            if (id <= 0) 
                return BadRequest();

            try
            {
                var challenge = await _challengeService.GetChallenge(id);
                return Ok(challenge);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion

        #region POST Actions
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateChallenge([FromBody] ChallengePostDto newChallenge)
        {
            await _challengeService.AddChallengeAsync(newChallenge);
            return Ok("Challenge Has been created");
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

            return Ok(new { Message = $"{challenge.Title} Challenge has started", StartDate = challenge.StartDate, EndDate = challenge.EndDate });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ChallengeGetDto>?> UpdateChallenge(int id, ChallengePostDto newChallenge)
        {
            if (id <= 0)
                return BadRequest("Invalid Id value");

            try
            {
                var challengeDto = await _challengeService.UpdateChallengeAsync(id, newChallenge);
                return Ok(challengeDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
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
            try
            {
                var chaDto = await _challengeService.CancelChallengeAsync(id);
                return Ok($"{chaDto?.Title} has been canceled.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region DELETE Actions
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ChallengeGetDto?>> DeleteChallenge(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid id value" });

            try
            {
                var deletedChallenge = await _challengeService.DeleteChallengeAsync(id);
                return Ok($"`{deletedChallenge?.Title}` challenge has been deleted");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        #endregion
    }
}
