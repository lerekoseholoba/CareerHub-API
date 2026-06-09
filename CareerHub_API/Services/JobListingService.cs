using CareerHub_API.DTOs;
using CareerHub_API.Exceptions;
using CareerHub_API.Models;
using CareerHub_API.Repositories;

namespace CareerHub_API.Services
{
    public class JobListingService : IJobListingService
    {
        private readonly IJobListingRepository _jobRepo;
        private readonly ICompanyRepository _companyRepo;

        public JobListingService(IJobListingRepository jobRepo, ICompanyRepository companyRepo)
        {
            _jobRepo = jobRepo;
            _companyRepo = companyRepo;
        }
        /*
        public async Task<PagedResponse<JobResponse>>
                                      GetAllAsync(
                                      int page,
                                      int pageSize)
        {
             return await _jobRepo
             .GetActiveListingsPagedAsync(
              page,
              pageSize);
        }
        */
        public async Task<PagedResponse<JobResponse>>GetAllAsync(
                                     JobListingFilterQuery query,
                                                        int page,
                                                    int pageSize)
       {
            return await _jobRepo
            .GetActiveListingsPagedAsync(
            query,
            page,
            pageSize);
       }
        public async Task<JobResponse> GetByIdAsync(Guid id)
        {
            var job = await _jobRepo.GetListingDetailsAsync(id);
            if (job == null) throw new Exception("Job not found");
            return job;
        }

        public async Task<JobResponse> CreateAsync(CreateJobRequest request)
        {
            // Rule: company must exist
            if (!await _companyRepo.ExistsAsync(request.CompanyId))
                throw new CompanyNotFoundException(request.CompanyId);

            // Rule: closing date must be in the future
            if (request.ClosingDate <= DateTime.UtcNow)
                throw new InvalidClosingDateException();

            var listing = new JobListing
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CompanyId = request.CompanyId,
                ClosingDate = request.ClosingDate,
                Location = request.Location,
                PostedDate = DateTime.UtcNow,
                IsOpen = true
            };

            await _jobRepo.AddAsync(listing);

            return await _jobRepo.GetListingDetailsAsync(listing.Id)
                   ?? throw new Exception("Failed to create job");
        }

        public async Task<JobResponse> UpdateAsync(Guid id, UpdateJobRequest request)
        {
            var job = await _jobRepo.GetListingDetailsAsync(id);
            if (job == null) throw new Exception("Job not found");

            // Rule: only owning company may update
            if (job.Company != (await _companyRepo.GetByIdAsync(request.CompanyId))?.Name)
                throw new UnauthorizedCompanyUpdateException();

            // Rule: closed listings cannot be modified
            if (!await _jobRepo.IsOpenAsync(id))
                throw new ListingClosedException();

            var updatedJob = new JobListing
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                CompanyId = request.CompanyId,
                ClosingDate = request.ClosingDate,
                Location = request.Location,
                PostedDate = DateTime.UtcNow,
                IsOpen = true
            };

            await _jobRepo.UpdateAsync(updatedJob);
            return await _jobRepo.GetListingDetailsAsync(id)
                   ?? throw new Exception("Failed to update job");
        }

        public async Task CloseAsync(Guid id)
        {
            if (!await _jobRepo.ExistsAsync(id))
                throw new Exception("Job not found");

            await _jobRepo.CloseAsync(id);
        }
    }
}