using CareerHub_API.DTOs;
using CareerHub_API.Models;
using CareerHub_API.Exceptions;

namespace CareerHub_API.Data;
public class JobService
{
    private static readonly List<JobListing> _jobs = new()
    {
        new JobListing
        {
            Id = 1,
            Title = "Backend Developer",
            Company = "TechCorp",
            Location = "Cape Town",
            Type = JobType.FullTime,
            Description = "Build APIs using .NET technologies.",
            SalaryMin = 25000,
            SalaryMax = 40000,
            PostedAt = DateTime.UtcNow,
            IsActive = true
        }
    };

    public Task<List<JobResponse>> GetAllJobsAsync()
    {
        return Task.FromResult(_jobs.Select(MapToResponse).ToList());
    }

    public Task<JobResponse?> GetJobByIdAsync(int id)
    {
        var job = _jobs.FirstOrDefault(j => j.Id == id);

        if (job == null)
        {
           throw new JobNotFoundException(id);
        }
        return Task.FromResult<JobResponse?>(MapToResponse(job));
    }

    public Task<JobResponse?> CreateJobAsync(CreateJobRequest request)
    {
        var duplicate = _jobs.Any(j =>
            j.Title.Equals(request.Title, StringComparison.OrdinalIgnoreCase)
            && j.Company.Equals(request.Company, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
          throw new DuplicateJobListingException(
          request.Title,
          request.Company);
        }

        var job = new JobListing
        {
            Id = _jobs.Max(j => j.Id) + 1,
            Title = request.Title,
            Company = request.Company,
            Location = request.Location,
            Description = request.Description,
            Type = request.Type,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            PostedAt = DateTime.UtcNow,
            IsActive = true
        };

        _jobs.Add(job);

        return Task.FromResult<JobResponse?>(MapToResponse(job));
    }

    public Task<JobResponse?> UpdateJobAsync(int id, UpdateJobRequest request)
    {
        var job = _jobs.FirstOrDefault(j => j.Id == id);

       if (job == null)
       {
        throw new JobNotFoundException(id);
       }
        job.Title = request.Title;
        job.Company = request.Company;
        job.Location = request.Location;
        job.Description = request.Description;
        job.Type = request.Type;
        job.SalaryMin = request.SalaryMin;
        job.SalaryMax = request.SalaryMax;

        return Task.FromResult<JobResponse?>(MapToResponse(job));
    }

    public Task DeleteJobAsync(int id)
    {
        var job = _jobs.FirstOrDefault(j => j.Id == id);

        if (job == null)
        {
         throw new JobNotFoundException(id);
        }

        _jobs.Remove(job);

        return Task.FromResult(true);
    }

    private static JobResponse MapToResponse(JobListing job)
    {
        return new JobResponse
        {
            Id = job.Id,
            Title = job.Title,
            Company = job.Company,
            Location = job.Location,
            Description = job.Description,
            Type = job.Type,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            PostedAt = job.PostedAt,
            IsActive = job.IsActive,
            SalaryDisplay = GetSalaryDisplay(job)
        };
    }

    private static string GetSalaryDisplay(JobListing job)
    {
        if (job.SalaryMin.HasValue && job.SalaryMax.HasValue)
        {
            return $"R{job.SalaryMin:N0} – R{job.SalaryMax:N0}/month";
        }

        if (job.SalaryMin.HasValue)
        {
            return $"From R{job.SalaryMin:N0}/month";
        }

        return "Salary not specified";
    }
}