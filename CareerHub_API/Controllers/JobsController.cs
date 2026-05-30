using CareerHub_API.Models;
using CareerHub_API.DTOs;
using CareerHub_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Web;

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

       return Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob(CreateJobRequest request)
    {
       var job = await _jobService.CreateJobAsync(request);

       return CreatedAtAction(
        nameof(GetJobById),
        new { id = job.Id },
        job);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(int id, UpdateJobRequest request)
    {
    var updatedJob =
        await _jobService.UpdateJobAsync(id, request);

    return Ok(updatedJob);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
    await _jobService.DeleteJobAsync(id);

    return NoContent();
    }

}