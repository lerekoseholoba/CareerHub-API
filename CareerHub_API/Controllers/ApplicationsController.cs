using CareerHub_API.DTOs;
using CareerHub_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ensures only authenticated users can apply/manage applications
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyToJob([FromBody] CreateApplicationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _applicationService.ApplyToJobAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Application });
        }

        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetApplicationsForJob(Guid jobId)
        {
            var result = await _applicationService.GetApplicationsForJobAsync(jobId);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(result.Applications);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetApplicationsForUser(Guid userId)
        {
            var result = await _applicationService.GetApplicationsForUserAsync(userId);

            if (!result.Success)
                return NotFound(new { message = result.Message });

            return Ok(result.Applications);
        }

        [HttpDelete("{applicationId}")]
        public async Task<IActionResult> WithdrawApplication(Guid applicationId)
        {
            var result = await _applicationService.WithdrawApplicationAsync(applicationId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}