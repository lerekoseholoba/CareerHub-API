using CareerHub_API.DTOs;
using CareerHub_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;

namespace CareerHub_API.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [EnableRateLimiting("apply")]
        [HttpPost]
        public async Task<IActionResult> ApplyToJob([FromBody] CreateApplicationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _applicationService.SubmitApplicationAsync(request);
                return Ok(new { message = "Application submitted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{jobId:guid}")]
        public async Task<IActionResult> WithdrawApplication(Guid jobId)
        {
            try
            {
                var applicantId = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException());
                await _applicationService.WithdrawAsync(applicantId, jobId, applicantId);
                return Ok(new { message = "Application withdrawn successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Recruiter")]
        [HttpPatch("{applicantId:guid}/{jobId:guid}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(
        Guid applicantId,
        Guid jobId,
        [FromBody] UpdateApplicationStatusRequest request)
       {
             try
            {
                var result =
                 await _applicationService
                .UpdateStatusAsync(
                    applicantId,
                    jobId,
                    request);

             return Ok(result);
           }
            catch (Exception ex)
         {
           return BadRequest(new
          {
            message = ex.Message
          });
        }
       }
    }
}
