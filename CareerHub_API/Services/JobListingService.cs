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
          if (!await _companyRepo.ExistsAsync(request.CompanyId))
          throw new CompanyNotFoundException(request.CompanyId);

          if (request.ClosingDate <= DateTime.UtcNow)
         throw new InvalidClosingDateException();

          if (request.SalaryMin.HasValue &&
         request.SalaryMax.HasValue &&
         request.SalaryMax <= request.SalaryMin)
          {
             throw new InvalidSalaryException();
          }

            var listing = new JobListing
          {
           Id = Guid.NewGuid(),
           Title = request.Title,
           Description = request.Description,
           CompanyId = request.CompanyId,
           ClosingDate = request.ClosingDate,
           Location = request.Location,
           PostedDate = DateTime.UtcNow,
           IsOpen = true,
           SalaryMin = request.SalaryMin ?? 0,
           SalaryMax = request.SalaryMax ?? 0
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
        //Patch method in service layer
       public async Task<JobResponse> PatchAsync(Guid id, UpdateJobListingRequest request)
      {
             var job = await _jobRepo.GetEntityByIdAsync(id);

            if (job == null)
               throw new JobNotFoundException(id);

           if (request.Title != null)
                 job.Title = request.Title;

           if (request.Description != null)
                job.Description = request.Description;

           if (request.Location != null)
                job.Location = request.Location;

           if (request.EmploymentType != null)
              job.EmploymentType = request.EmploymentType;

           var newMin = request.SalaryMin ?? job.SalaryMin;
           var newMax = request.SalaryMax ?? job.SalaryMax;

          if ((request.SalaryMin.HasValue || request.SalaryMax.HasValue)
             && newMin > newMax)
           {
              throw new InvalidSalaryException();
           }

         if (request.SalaryMin.HasValue)
            job.SalaryMin = request.SalaryMin.Value;

         if (request.SalaryMax.HasValue)
            job.SalaryMax = request.SalaryMax.Value;

         if (request.ExpiresAt.HasValue)
           {
              if (request.ExpiresAt.Value <= DateTime.UtcNow)
              throw new InvalidClosingDateException();

              job.ClosingDate = request.ExpiresAt.Value;
           }

          await _jobRepo.UpdateAsync(job);

          return await _jobRepo.GetListingDetailsAsync(id)
           ?? throw new JobNotFoundException(id);
       }
    }
}