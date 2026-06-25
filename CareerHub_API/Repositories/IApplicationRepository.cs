using CareerHub_API.Models;
using CareerHub_API.DTOs;

namespace CareerHub_API.Repositories
{
    public interface IApplicationRepository
    {
        Task<bool> ApplicantAlreadyAppliedAsync(Guid applicantId, Guid jobListingId);

        Task<List<Application>> GetApplicationsForListingAsync(Guid jobListingId);

        Task<List<Application>> GetApplicationsByApplicantAsync(Guid applicantId);

        Task<Application?> GetByIdAsync(Guid applicantId, Guid jobListingId);

        Task AddAsync(Application application);

        Task UpdateStatusAsync(Application application);
        Task<IEnumerable<ApplicationStatsResponse>> GetApplicationStatsAsync();
    }
}