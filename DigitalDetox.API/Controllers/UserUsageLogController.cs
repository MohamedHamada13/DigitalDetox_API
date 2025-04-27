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
            if(!result.IsLogged)
                return BadRequest(result.Message);

            return Ok(result);
        }

        // Important Note: these endpoints can be of httpPost type if want to send the request from body not url.
        [HttpGet("Daily")]
        public async Task<IActionResult> GetDailyLogsAsync([FromBody] DateOnly dayDate)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dailyLogService.GetDailyLogsAsync(dayDate);
            if(result == null)
                return BadRequest($"Invalid or expired token");

            return Ok(result);
        }


        [HttpGet("InRange")]
        public async Task<IActionResult> GetLogsInRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            var result = await _dailyLogService.GetLogsInRangeAsync(startDate, endDate);
            if (result == null)
                return BadRequest("Invalid of expired token");

            return Ok(result);
        }
    }
}
