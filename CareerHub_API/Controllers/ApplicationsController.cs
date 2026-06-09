using CareerHub_API.DTOs;
using CareerHub_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
    }
}
