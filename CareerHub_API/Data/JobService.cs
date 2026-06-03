using CareerHub_API.DTOs;
using CareerHub_API.Models;
using CareerHub_API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CareerHub_API.Data;

public class JobService
{
    private readonly CareerHubDbContext _context;

    public JobService(CareerHubDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobResponse>> GetAllJobsAsync()
    {
        var jobs = await _context.JobListings
            .Include(j => j.Company)
            .AsNoTracking()
            .ToListAsync();

        return jobs.Select(MapToResponse).ToList();
    }

    public async Task<JobResponse> GetJobByIdAsync(Guid id)
    {
        var job = await _context.JobListings
            .Include(j => j.Company)
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
            throw new JobNotFoundException(id);

        return MapToResponse(job);
    }

    public async Task<JobResponse> CreateJobAsync(CreateJobRequest request)
    {
        var duplicate = await _context.JobListings.AnyAsync(j =>
            j.Title.ToLower() == request.Title.ToLower() &&
            j.Company.Name.ToLower() == request.Company.ToLower());

        if (duplicate)
        {
            throw new DuplicateJobListingException(request.Title, request.Company);
        }

        var job = new JobListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Company = new Company
            {
                Name = request.Company
            },
            Location = request.Location,
            Description = request.Description,
            PostedDate = DateTime.UtcNow
        };

        _context.JobListings.Add(job);
        await _context.SaveChangesAsync();

        return MapToResponse(job);
    }

    public async Task<JobResponse> UpdateJobAsync(Guid id, UpdateJobRequest request)
    {
        var job = await _context.JobListings
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
            throw new JobNotFoundException(id);

        job.Title = request.Title;
        job.Company.Name = request.Company;
        job.Location = request.Location;
        job.Description = request.Description;

        await _context.SaveChangesAsync();

        return MapToResponse(job);
    }

    public async Task DeleteJobAsync(Guid id)
    {
        var job = await _context.JobListings.FindAsync(id);

        if (job == null)
            throw new JobNotFoundException(id);

        _context.JobListings.Remove(job);
        await _context.SaveChangesAsync();
    }

    private static JobResponse MapToResponse(JobListing job)
    {
        return new JobResponse
        {
            Id = job.Id,
            Title = job.Title,
            Company = job.Company.Name,
            Location = job.Location,
            Description = job.Description,
            PostedAt = job.PostedDate
        };
    }
}