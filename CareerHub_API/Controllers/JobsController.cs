using CareerHub_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub_Api.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        var jobs = await _jobService.GetAllJobsAsync();
        return Ok(jobs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        var job = await _jobService.GetJobByIdAsync(id);

        if (job == null)
            return NotFound();

        return Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob(CreateJobRequest request)
    {
        var job = await _jobService.CreateJobAsync(request);

        if (job == null)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Duplicate job listing",
                Detail = "A job with the same title and company already exists.",
                Status = 409
            });
        }

        return CreatedAtAction(
            nameof(GetJobById),
            new { id = job.Id },
            job);
    }

}