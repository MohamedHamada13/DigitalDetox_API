using DigitalDetox.Application.Servicies;
using DigitalDetox.Core.DTOs.DailyUsageLogDtos;
using DigitalDetox.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetox.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserUsageLogController : ControllerBase
    {
        private readonly IDailyUsageLogService _dailyLogService;
        public UserUsageLogController(IDailyUsageLogService dailyLogService)
        {
            _dailyLogService = dailyLogService;
        }

        [HttpPost]
        public async Task<IActionResult> DailyLogAsync([FromBody] DailyLogRequest model)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var result = await _dailyLogService.LogUsageAsync(model);
            if(!result.IsLoged)
                return BadRequest(result.Message);

            return Ok(result);
        }


        // Important Note: these endpoints can be of httpPost type if want to send the request from body not url.
        [HttpGet("Daily")]
        public async Task<IActionResult> GetDailyLogsAsync([FromQuery] string userId, [FromQuery] DateOnly dayDate)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dailyLogService.GetDailyLogsAsync(userId, dayDate);
            if(result == null)
                return BadRequest($"User id or date is not correct");

            return Ok(result);
        }


        [HttpGet("InRange")]
        public async Task<IActionResult> GetLogsInRangeAsync([FromQuery] string userId, DateOnly startDate, DateOnly endDate)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User id is required");

            var result = await _dailyLogService.GetLogsInRangeAsync(userId, startDate, endDate);
            if (result == null)
                return BadRequest("User id is incorrect");

            return Ok(result);
        }
    }
}
