using CareerHub_API.Data;
using CareerHub_API.Models;
using CareerHub_API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CareerHub_API.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly CareerHubDbContext _context;

        public ApplicationRepository(CareerHubDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ApplicantAlreadyAppliedAsync(Guid applicantId, Guid jobListingId)
        {
            return await _context.Applications
                .AnyAsync(a => a.ApplicantId == applicantId && a.JobListingId == jobListingId);
        }

        public async Task<List<Application>> GetApplicationsForListingAsync(Guid jobListingId)
        {
            return await _context.Applications
                .Where(a => a.JobListingId == jobListingId)
                .Include(a => a.Applicant)
                .ToListAsync();
        }

        public async Task<List<Application>> GetApplicationsByApplicantAsync(Guid applicantId)
        {
            return await _context.Applications
                .Where(a => a.ApplicantId == applicantId)
                .Include(a => a.JobListing)
                .ToListAsync();
        }

        public async Task<Application?> GetByIdAsync(Guid applicantId, Guid jobListingId)
        {
            return await _context.Applications
                .Include(a => a.JobListing)
                .Include(a => a.Applicant)
                .FirstOrDefaultAsync(a =>
                    a.ApplicantId == applicantId &&
                    a.JobListingId == jobListingId);
        }

        public async Task AddAsync(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Application application)
        {
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ApplicationStatsResponse>> GetApplicationStatsAsync()
        {
           return await _context.Applications
               .GroupBy(a => a.JobListingId)
               .Select(g => new ApplicationStatsResponse
           {
            JobId = g.Key,
            ApplicationCount = g.Count()
           })
               .ToListAsync();
        }
    }
}
