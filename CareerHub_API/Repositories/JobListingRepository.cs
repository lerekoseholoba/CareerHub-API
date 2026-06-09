using CareerHub_API.Data;
using CareerHub_API.DTOs;
using CareerHub_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerHub_API.Repositories
{
    public class JobListingRepository : IJobListingRepository
    {
        private readonly CareerHubDbContext _context;

        public JobListingRepository(CareerHubDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<JobResponse>>
                     GetActiveListingsPagedAsync(
                     int page,
                      int pageSize)
        {
             var query = _context.JobListings
             .Where(j => j.IsOpen)
             .OrderByDescending(j => j.PostedDate);

             var totalCount = await query.CountAsync();

             var jobs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(j => new JobResponse
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Company = j.Company.Name,
                    Location = j.Location,
                    PostedAt = j.PostedDate,
                    ApplicationCount =
                    j.Applications.Count()
                })
                 .ToListAsync();

            return new PagedResponse<JobResponse>
            {
                    Data = jobs,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount
            };
        }

        public async Task<JobResponse?> GetListingDetailsAsync(Guid id)
        {
            return await _context.JobListings
                .Where(j => j.Id == id)
                .Select(j => new JobResponse
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Company = j.Company.Name,
                    Location = j.Location,
                    PostedAt = j.PostedDate,
                    ApplicationCount = j.Applications.Count()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.JobListings.AnyAsync(j => j.Id == id);
        }

        public async Task<bool> IsOpenAsync(Guid id)
        {
            var job = await _context.JobListings
                .Where(j => j.Id == id)
                .Select(j => j.IsOpen)
                .FirstOrDefaultAsync();

            return job;
        }

        public async Task AddAsync(JobListing listing)
        {
            _context.JobListings.Add(listing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobListing listing)
        {
            _context.JobListings.Update(listing);
            await _context.SaveChangesAsync();
        }

        public async Task CloseAsync(Guid id)
        {
            var job = await _context.JobListings.FindAsync(id);
            if (job != null)
            {
                job.IsOpen = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}