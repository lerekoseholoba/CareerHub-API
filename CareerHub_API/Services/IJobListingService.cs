using CareerHub_API.DTOs;
using CareerHub_API.Models;

namespace CareerHub_API.Services
{
    public interface IJobListingService
    {
        Task<List<JobResponse>> GetAllAsync();
        Task<JobResponse> GetByIdAsync(Guid id);
        Task<JobResponse> CreateAsync(CreateJobRequest request);
        Task<JobResponse> UpdateAsync(Guid id, UpdateJobRequest request);
        Task CloseAsync(Guid id);
    }
}
