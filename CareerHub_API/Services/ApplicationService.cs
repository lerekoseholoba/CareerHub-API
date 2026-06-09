using CareerHub_API.DTOs;
using CareerHub_API.Exceptions;
using CareerHub_API.Models;
using CareerHub_API.Repositories;
using CareerHub_API.Utilities;

namespace CareerHub_API.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _appRepo;
        private readonly IJobListingRepository _jobRepo;

        public ApplicationService(IApplicationRepository appRepo, IJobListingRepository jobRepo)
        {
            _appRepo = appRepo;
            _jobRepo = jobRepo;
        }

        public async Task SubmitApplicationAsync(CreateApplicationRequest request)
        {
            // Rule: cannot apply after closing
            if (!await _jobRepo.IsOpenAsync(request.JobListingId))
                throw new ListingClosedException();

            // Rule: cannot apply twice
            if (await _appRepo.ApplicantAlreadyAppliedAsync(request.ApplicantId, request.JobListingId))
                throw new DuplicateApplicationException();
                
            var application = new Application
            {
                ApplicantId = request.ApplicantId,
                JobListingId = request.JobListingId,
                ResumeUrl = request.ResumeUrl,
                CoverLetter = request.CoverLetter,
                Status = ApplicationStatus.Submitted,
                SubmittedAt = DateTime.UtcNow
            };

            await _appRepo.AddAsync(application);
        }

        public async Task UpdateStatusAsync(Guid applicantId, Guid jobId, ApplicationStatus status)
        {
            var app = await _appRepo.GetByIdAsync(applicantId, jobId)
                      ?? throw new Exception("Application not found");

            if (!ApplicationStatusRules.IsValidTransition(app.Status, status))
                throw new InvalidStatusTransitionException(app.Status, status);

            app.Status = status;
            await _appRepo.UpdateStatusAsync(app);
        }

        public async Task WithdrawAsync(Guid applicantId, Guid jobId, Guid currentUserApplicantId)
        {
            var app = await _appRepo.GetByIdAsync(applicantId, jobId)
                      ?? throw new Exception("Application not found");

            if (app.ApplicantId != currentUserApplicantId)
                throw new UnauthorizedApplicationAccessException();

            app.Status = ApplicationStatus.Withdrawn;
            await _appRepo.UpdateStatusAsync(app);
        }
        public async Task<ApplicationResponse> UpdateStatusAsync(Guid applicantId,Guid jobId,
                                                         UpdateApplicationStatusRequest request)
        {
            var application =
            await _appRepo.GetByIdAsync(
            applicantId,
            jobId);

           if (application == null)
           throw new Exception("Application not found");

           if ((application.Status == ApplicationStatus.Rejected ||
           application.Status == ApplicationStatus.Offered)
           &&
           request.Status == ApplicationStatus.Submitted)
         {
            throw new Exception(
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