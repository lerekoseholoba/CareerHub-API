using CareerHub_API.DTOs;
using CareerHub_API.Models;

namespace CareerHub_API.Repositories
{
    public interface IJobListingRepository
    {
        Task<List<JobResponse>> GetActiveListingsAsync();

        Task<JobResponse?> GetListingDetailsAsync(Guid id);

        Task<bool> ExistsAsync(Guid id);

        Task<bool> IsOpenAsync(Guid id);

        Task AddAsync(JobListing listing);

        Task UpdateAsync(JobListing listing);

        Task CloseAsync(Guid id);
    }
}