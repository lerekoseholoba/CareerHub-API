using CareerHub_API.DTOs;
using CareerHub_API.Models;
using CareerHub_API.Repositories;

namespace CareerHub_API.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _appRepo;
        private readonly IJobListingRepository _jobRepo;

        public ApplicationService(
            IApplicationRepository appRepo,
            IJobListingRepository jobRepo)
        {
            _appRepo = appRepo;
            _jobRepo = jobRepo;
        }

        public async Task SubmitApplicationAsync(CreateApplicationRequest request)
        {
            // Cannot apply after closing
            if (!await _jobRepo.IsOpenAsync(request.JobListingId))
                throw new InvalidOperationException("Job listing is closed");

            // Cannot apply twice
            if (await _appRepo.ApplicantAlreadyAppliedAsync(
                request.ApplicantId,
                request.JobListingId))
            {
                throw new InvalidOperationException(
                    "You have already applied to this job");
            }

            var application = new Application
            {
                ApplicantId = request.ApplicantId,
                JobListingId = request.JobListingId,
                Status = ApplicationStatus.Submitted,
                SubmittedAt = DateTime.UtcNow
            };

            await _appRepo.AddAsync(application);
        }

        public async Task UpdateStatusAsync(
            Guid applicantId,
            Guid jobId,
            ApplicationStatus status)
        {
            var application =
                await _appRepo.GetByIdAsync(applicantId, jobId);

            if (application == null)
                throw new KeyNotFoundException("Application not found");

            application.TransitionTo(status);

            await _appRepo.UpdateStatusAsync(application);
        }

        public async Task WithdrawAsync(
            Guid applicantId,
            Guid jobId,
            Guid currentUserApplicantId)
        {
            if (applicantId != currentUserApplicantId)
                throw new UnauthorizedAccessException(
                    "You can only withdraw your own application");

            var application =
                await _appRepo.GetByIdAsync(applicantId, jobId);

            if (application == null)
                throw new KeyNotFoundException("Application not found");

            application.TransitionTo(ApplicationStatus.Rejected);

            await _appRepo.UpdateStatusAsync(application);
        }

        public async Task<ApplicationResponse> UpdateStatusAsync(
            Guid applicantId,
            Guid jobId,
            UpdateApplicationStatusRequest request)
        {
            var application =
                await _appRepo.GetByIdAsync(applicantId, jobId);

            if (application == null)
                throw new KeyNotFoundException("Application not found");

            if ((application.Status == ApplicationStatus.Rejected ||
                 application.Status == ApplicationStatus.Offered) &&
                 request.Status == ApplicationStatus.Submitted)
            {
                throw new InvalidOperationException(
                    "Cannot move Rejected or Offered application back to Submitted");
            }

            application.Status = request.Status;

            await _appRepo.UpdateStatusAsync(application);

            return new ApplicationResponse
            {
                ApplicantId = application.ApplicantId,
                JobListingId = application.JobListingId,
                ApplicantName = application.Applicant.Name,
                JobTitle = application.JobListing.Title,
                Status = application.Status,
                SubmittedAt = application.SubmittedAt
            };
        }
    }
}