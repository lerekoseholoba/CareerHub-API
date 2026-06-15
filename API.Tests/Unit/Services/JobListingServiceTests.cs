using CareerHub_API.DTOs;
using CareerHub_API.Exceptions;
using CareerHub_API.Models;
using CareerHub_API.Repositories;
using CareerHub_API.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests.Unit.Services
{
    public class JobListingServiceTests
    {
        private readonly IJobListingRepository _jobRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly JobListingService _service;

        public JobListingServiceTests()
        {
            _jobRepo = Substitute.For<IJobListingRepository>();
            _companyRepo = Substitute.For<ICompanyRepository>();
            _service = new JobListingService(_jobRepo, _companyRepo);
        }

        [Fact]
        public async Task CreateAsync_WhenSalaryMaxLessThanSalaryMin_ThrowsInvalidSalaryException()
        {
            var request = new CreateJobRequest
            {
                CompanyId = Guid.NewGuid(),
                Title = "Developer",
                Location = "Cape Town",
                Description = "Some long description for the job",
                SalaryMin = 80000,
                SalaryMax = 50000,
                ClosingDate = DateTime.UtcNow.AddDays(5)
            };

            _companyRepo.ExistsAsync(request.CompanyId).Returns(true);

            await Assert.ThrowsAsync<InvalidSalaryException>(() => _service.CreateAsync(request));

            await _jobRepo.DidNotReceive().AddAsync(Arg.Any<JobListing>());
        }

        [Fact]
        public async Task CreateAsync_WhenClosingDateIsInThePast_ThrowsInvalidClosingDateException()
        {
            var request = new CreateJobRequest
            {
                CompanyId = Guid.NewGuid(),
                Title = "Developer",
                Location = "Cape Town",
                Description = "Some long description for the job",
                SalaryMin = 50000,
                SalaryMax = 80000,
                ClosingDate = DateTime.UtcNow.AddDays(-1)
            };

            _companyRepo.ExistsAsync(request.CompanyId).Returns(true);

            await Assert.ThrowsAsync<InvalidClosingDateException>(() => _service.CreateAsync(request));

            await _jobRepo.DidNotReceive().AddAsync(Arg.Any<JobListing>());
        }

        [Fact]
        public async Task CreateAsync_WhenValid_CallsAddAsyncExactlyOnce()
        {
            var request = new CreateJobRequest
            {
                CompanyId = Guid.NewGuid(),
                Title = "Developer",
                Location = "Cape Town",
                Description = "Some long description for the job",
                SalaryMin = 50000,
                SalaryMax = 80000,
                ClosingDate = DateTime.UtcNow.AddDays(5)
            };

            _companyRepo.ExistsAsync(request.CompanyId).Returns(true);
            _jobRepo.GetListingDetailsAsync(Arg.Any<Guid>()).Returns(new JobResponse());

            await _service.CreateAsync(request);

            await _jobRepo.Received(1).AddAsync(Arg.Any<JobListing>());
        }

        [Fact]
        public async Task PatchAsync_WhenOnlySalaryMinChanged_ThrowsInvalidSalaryExceptionIfMinExceedsMax()
        {
            var existingJob = new JobListing
            {
                Id = Guid.NewGuid(),
                SalaryMin = 50000,
                SalaryMax = 80000
            };

            _jobRepo.GetEntityByIdAsync(existingJob.Id).Returns(existingJob);

            var request = new UpdateJobListingRequest
            {
                SalaryMin = 90000
            };

            await Assert.ThrowsAsync<InvalidSalaryException>(
                () => _service.PatchAsync(existingJob.Id, request));

            await _jobRepo.DidNotReceive().UpdateAsync(Arg.Any<JobListing>());
        }

        [Fact]
        public async Task PatchAsync_WhenOnlyTitleChanged_CallsUpdateAsyncWithoutSalaryValidation()
        {
            var existingJob = new JobListing
            {
                Id = Guid.NewGuid(),
                Title = "Old Title",
                SalaryMin = 50000,
                SalaryMax = 80000
            };

            _jobRepo.GetEntityByIdAsync(existingJob.Id).Returns(existingJob);
            _jobRepo.GetListingDetailsAsync(existingJob.Id)
                                                    .Returns(new JobResponse());

            var request = new UpdateJobListingRequest
            {
                Title = "New Title"
            };

            await _service.PatchAsync(existingJob.Id, request);

            await _jobRepo.Received(1).UpdateAsync(existingJob);
        }

        [Fact]
        public async Task PatchAsync_WhenListingNotFound_ThrowsJobNotFoundException()
        {
            var jobId = Guid.NewGuid();
            _jobRepo.GetEntityByIdAsync(jobId).Returns((JobListing?)null);

            var request = new UpdateJobListingRequest
            {
                Title = "New Title"
            };

            await Assert.ThrowsAsync<JobNotFoundException>(
                () => _service.PatchAsync(jobId, request));

            await _jobRepo.DidNotReceive().UpdateAsync(Arg.Any<JobListing>());
        }
    }
}