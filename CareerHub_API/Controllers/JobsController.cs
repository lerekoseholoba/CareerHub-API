using CareerHub_API.DTOs;
using CareerHub_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CareerHub_API.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        var jobs = await _jobService.GetAllJobsAsync();
        return Ok(jobs);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(Guid id)
    {
        var job = await _jobService.GetJobByIdAsync(id);
        return Ok(job);
    }

    [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> CreateJob(CreateJobRequest request)
    {
        var job = await _jobService.CreateJobAsync(request);

        return CreatedAtAction(
            nameof(GetJobById),
            new { id = job.Id },
            job
        );
    }

    [Authorize(Roles = "Employer")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(Guid id, UpdateJobRequest request)
    {
        var job = await _jobService.UpdateJobAsync(id, request);
        return Ok(job);
    }

    [Authorize(Roles = "Employer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        await _jobService.DeleteJobAsync(id);
        return NoContent();
    }
}