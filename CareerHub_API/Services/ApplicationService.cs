using CareerHub_API.DTOs;
using CareerHub_API.Models;
using CareerHub_API.Repositories;

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

        public async Task<ServiceResult<Application>> ApplyToJobAsync(CreateApplicationRequest request)
        {
            try
            {
                // Rule: cannot apply after closing
                if (!await _jobRepo.IsOpenAsync(request.JobListingId))
                    return ServiceResult<Application>.Failure("Job listing is closed");

                // Rule: cannot apply twice
                if (await _appRepo.ApplicantAlreadyAppliedAsync(request.ApplicantId, request.JobListingId))
                    return ServiceResult<Application>.Failure("You have already applied to this job");

                var application = new Application
                {
                    ApplicantId = request.ApplicantId,
                    JobListingId = request.JobListingId,
                    Status = ApplicationStatus.Submitted,
                    SubmittedAt = DateTime.UtcNow
                };

                await _appRepo.AddAsync(application);
                return ServiceResult<Application>.Success("Application submitted successfully", application);
            }
            catch (Exception ex)
            {
                return ServiceResult<Application>.Failure(ex.Message);
            }
        }

        public async Task<ServiceResult<List<Application>>> GetApplicationsForJobAsync(Guid jobId)
        {
            try
            {
                var applications = await _appRepo.GetApplicationsForListingAsync(jobId);
                return ServiceResult<List<Application>>.Success("Applications retrieved", applications);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Application>>.Failure(ex.Message);
            }
        }

        public async Task<ServiceResult<List<Application>>> GetApplicationsForUserAsync(Guid userId)
        {
            try
            {
                var applications = await _appRepo.GetApplicationsByApplicantAsync(userId);
                return ServiceResult<List<Application>>.Success("Applications retrieved", applications);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<Application>>.Failure(ex.Message);
            }
        }

        public async Task<ServiceResult<string>> WithdrawApplicationAsync(Guid applicationId)
        {
            try
            {
                var applications = await _appRepo.GetApplicationsByApplicantAsync(applicationId);
                if (applications.Count == 0)
                    return ServiceResult<string>.Failure("Application not found");

                var application = applications.First();
                application.TransitionTo(ApplicationStatus.Rejected);
                await _appRepo.UpdateStatusAsync(application);
                return ServiceResult<string>.Success("Application withdrawn successfully", "Success");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure(ex.Message);
            }
        }
    }
}
