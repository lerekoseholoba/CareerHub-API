using CareerHub_API.Data;
using CareerHub_API.DTOs;
using CareerHub_API.Models;
using Microsoft.EntityFrameworkCore;
using CareerHub_API.Exceptions;

namespace CareerHub_API.Repositories
{
    public class JobListingRepository : IJobListingRepository
    {
        private readonly CareerHubDbContext _context;

        public JobListingRepository(CareerHubDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<JobResponse>> GetActiveListingsPagedAsync(
            JobListingFilterQuery filter,
            int page,
            int pageSize)
        {
            IQueryable<JobListing> query = _context.JobListings
                .Include(j => j.Company)
                .Include(j => j.Applications)
                .Where(j => j.IsOpen);

            // =====================
            // Filters
            // =====================

            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                query = query.Where(j =>
                    j.Location.ToLower()
                        .Contains(filter.Location.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.EmploymentType))
            {
                query = query.Where(j =>
                    j.EmploymentType == filter.EmploymentType);
            }

            if (filter.SalaryMin.HasValue)
            {
                query = query.Where(j =>
                    j.SalaryMin >= filter.SalaryMin.Value);
            }

            if (filter.SalaryMax.HasValue)
            {
                query = query.Where(j =>
                    j.SalaryMax <= filter.SalaryMax.Value);
            }

            if (filter.CompanyId.HasValue)
            {
                query = query.Where(j =>
                    j.CompanyId == filter.CompanyId.Value);
            }

            // =====================
            // Sorting
            // =====================

            var dir = filter.Dir?.ToLower();

            query = filter.Sort.ToLower() switch
            {
                "salarymin" =>
                    dir == "desc"
                        ? query.OrderByDescending(j => j.SalaryMin)
                        : query.OrderBy(j => j.SalaryMin),

                "salarymax" =>
                    dir == "asc"
                        ? query.OrderBy(j => j.SalaryMax)
                        : query.OrderByDescending(j => j.SalaryMax),

                "title" =>
                    dir == "desc"
                        ? query.OrderByDescending(j => j.Title)
                        : query.OrderBy(j => j.Title),

                "postedat" =>
                    dir == "asc"
                        ? query.OrderBy(j => j.PostedDate)
                        : query.OrderByDescending(j => j.PostedDate),

                _ =>
                    query.OrderByDescending(j => j.PostedDate)
            };

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
                    SalaryMin = j.SalaryMin,
                    ApplicationCount = j.Applications.Count()
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
                    SalaryMin = j.SalaryMin,
                    ApplicationCount = j.Applications.Count()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.JobListings
                .AnyAsync(j => j.Id == id);
        }

        public async Task<bool> IsOpenAsync(Guid id)
        {
            return await _context.JobListings
                .Where(j => j.Id == id)
                .Select(j => j.IsOpen)
                .FirstOrDefaultAsync();
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

        // =====================
        // PATCH
        // =====================

        public async Task<JobResponse> PatchAsync(
            Guid id,
            UpdateJobListingRequest request)
        {
            var job = await _context.JobListings
                .Include(j => j.Company)
                .Include(j => j.Applications)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
                throw new Exception("Job not found");

            if (request.Title != null)
                job.Title = request.Title;

            if (request.Description != null)
                job.Description = request.Description;

            if (request.Location != null)
                job.Location = request.Location;

            if (request.EmploymentType != null)
                job.EmploymentType = request.EmploymentType;

            if (request.SalaryMin.HasValue)
                job.SalaryMin = request.SalaryMin.Value;

            if (request.SalaryMax.HasValue)
                job.SalaryMax = request.SalaryMax.Value;

            if (request.ExpiresAt.HasValue)
            {
                if (request.ExpiresAt <= DateTime.UtcNow)
                    throw new InvalidClosingDateException();

                job.ClosingDate = request.ExpiresAt.Value;
            }

            if (request.SalaryMin.HasValue || request.SalaryMax.HasValue)
            {
                if (job.SalaryMin > job.SalaryMax)
                {
                    throw new Exception(
                        "SalaryMin cannot be greater than SalaryMax");
                }
            }

            await _context.SaveChangesAsync();

            return new JobResponse
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Company = job.Company.Name,
                Location = job.Location,
                PostedAt = job.PostedDate,
                SalaryMin = job.SalaryMin,
                ApplicationCount = job.Applications.Count()
            };
        }
    }
}