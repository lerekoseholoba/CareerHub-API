using CareerHub_Api.Models;

public class JobService
{
    private static readonly List<JobListing> _jobs = new()
    {
        new JobListing { Id = 1, Title = "Backend Developer", Company = "TechCorp", Location = "Cape Town", Type = "Full-time", Description = "Build APIs using .NET" },
        new JobListing { Id = 2, Title = "Frontend Developer", Company = "WebWorks", Location = "Johannesburg", Type = "Internship", Description = "Work with React apps" },
        new JobListing { Id = 3, Title = "Mobile Developer", Company = "AppStudio", Location = "Remote", Type = "Contract", Description = "Build Flutter apps" }
    };

    public Task<List<JobListing>> GetAllJobsAsync()
    {
        return Task.FromResult(_jobs);
    }

    public Task<JobListing?> GetJobByIdAsync(int id)
    {
        var job = _jobs.FirstOrDefault(j => j.Id == id);
        return Task.FromResult(job);
    }
}