using CareerHub_API.DTOs;
using CareerHub_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub_API.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobListingService _jobListingService;

    public JobsController(IJobListingService jobListingService)
    {
        _jobListingService = jobListingService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllJobs(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        if (page < 1)
          page = 1;

        if (pageSize < 1)
          pageSize = 20;

        var result =
          await _jobListingService
            .GetAllAsync(
                page,
                pageSize);

          Response.Headers["X-Total-Count"] =
          result.TotalCount.ToString();

        return Ok(result);
   }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetJobById(Guid id)
    {
        var job = await _jobListingService.GetByIdAsync(id);
        return Ok(job);
    }

    [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request)
    {
        var job = await _jobListingService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetJobById),
            new { id = job.Id },
            job
        );
    }

    [Authorize(Roles = "Employer")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobRequest request)
    {
        var job = await _jobListingService.UpdateAsync(id, request);
        return Ok(job);
    }

    [Authorize(Roles = "Employer")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        await _jobListingService.CloseAsync(id);
        return NoContent();
    }
}