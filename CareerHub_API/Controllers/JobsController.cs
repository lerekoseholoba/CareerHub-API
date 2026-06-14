using System.Security.Cryptography;
using System.Text;
using CareerHub_API.DTOs;
using CareerHub_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;

namespace CareerHub_API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/jobs")]
[Route("api/v{version:apiVersion}/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobListingService _jobListingService;
    private readonly ILogger<JobsController> _logger;

    public JobsController(IJobListingService jobListingService, ILogger<JobsController> logger)
    {
        _jobListingService = jobListingService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllJobs(
       [FromQuery] string? location,
       [FromQuery] string? employmentType,
       [FromQuery] decimal? salaryMin,
       [FromQuery] decimal? salaryMax,
       [FromQuery] Guid? companyId,
       [FromQuery] string sort = "postedAt",
       [FromQuery] string? dir = null,
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 20)
    {
        if (page < 1) return BadRequest("Page must be greater than 0.");
        if (pageSize < 1) return BadRequest("PageSize must be greater than 0.");

        var filter = new JobListingFilterQuery
        {
            Location = location,
            EmploymentType = employmentType,
            SalaryMin = salaryMin,
            SalaryMax = salaryMax,
            CompanyId = companyId,
            Sort = sort,
            Dir = dir
        };

        try
        {
            var result = await _jobListingService.GetAllAsync(filter, page, pageSize);

            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            Response.Headers["Api-Supported-Versions"] = "1.0";

            return Ok(result);
        }
       catch (Exception ex)
        {
             _logger.LogError(ex, "Error fetching jobs list");

             return StatusCode(
               StatusCodes.Status500InternalServerError,
               ex.ToString());
        }
    }

    [AllowAnonymous]
    [EnableRateLimiting("search")]
    [HttpGet("search")]
    public async Task<IActionResult> SearchJobs(
        [FromQuery] string? location,
        [FromQuery] string? employmentType,
        [FromQuery] decimal? salaryMin,
        [FromQuery] decimal? salaryMax,
        [FromQuery] Guid? companyId,
        [FromQuery] string sort = "postedAt",
        [FromQuery] string? dir = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filter = new JobListingFilterQuery
        {
            Location = location,
            EmploymentType = employmentType,
            SalaryMin = salaryMin,
            SalaryMax = salaryMax,
            CompanyId = companyId,
            Sort = sort,
            Dir = dir
        };

        try
        {
            var result = await _jobListingService.GetAllAsync(filter, page, pageSize);

            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            Response.Headers["Api-Supported-Versions"] = "1.0";

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching jobs");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetJobById(Guid id)
    {
        try
        {
            var job = await _jobListingService.GetByIdAsync(id);
            if (job == null) return NotFound();

            var etag = GenerateETag(job);
            var clientETag = Request.Headers.IfNoneMatch.FirstOrDefault();

            if (!string.IsNullOrEmpty(clientETag) &&
                clientETag.Trim('"') == etag.Trim('"'))
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            Response.Headers.ETag = etag;
            return Ok(job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching job {JobId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [Authorize(Roles = "Employer")]
    [EnableRateLimiting("post-listing")]
    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request)
    {
        try
        {
            var job = await _jobListingService.CreateAsync(request);

            return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [Authorize(Roles = "Employer")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobRequest request)
    {
        try
        {
            var job = await _jobListingService.UpdateAsync(id, request);
            if (job == null) return NotFound();

            return Ok(job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job {JobId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [Authorize(Roles = "Employer")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        try
        {
            await _jobListingService.CloseAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job {JobId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [Authorize(Roles = "Employer")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PatchJob(Guid id, [FromBody] UpdateJobListingRequest request)
    {
        try
        {
            var result = await _jobListingService.PatchAsync(id, request);
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error patching job {JobId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    private static string GenerateETag(JobResponse job)
    {
        var content =
            $"{job.Id}-{job.Title}-{job.Description}-{job.Company}-{job.Location}-{job.PostedAt:O}-{job.SalaryMin}-{job.ApplicationCount}";

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(content));

        return $"\"{Convert.ToBase64String(hash)}\"";
    }
}
