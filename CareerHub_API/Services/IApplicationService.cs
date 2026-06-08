using CareerHub_API.DTOs;
using CareerHub_API.Models;

namespace CareerHub_API.Services
{
    public interface IApplicationService
    {
        Task<ServiceResult<Application>> ApplyToJobAsync(CreateApplicationRequest request);
        Task<ServiceResult<List<Application>>> GetApplicationsForJobAsync(Guid jobId);
        Task<ServiceResult<List<Application>>> GetApplicationsForUserAsync(Guid userId);
        Task<ServiceResult<string>> WithdrawApplicationAsync(Guid applicationId);
    }
}
