using CareerHub_API.DTOs;
using CareerHub_API.Models;

namespace CareerHub_API.Repositories
{
    public interface IJobListingRepository
    {
        //Task<List<JobResponse>> GetActiveListingsAsync();
        /*
        Task<PagedResponse<JobResponse>>GetActiveListingsPagedAsync(
        int page,
        int pageSize);
        */
        Task<JobResponse?> GetListingDetailsAsync(Guid id);
        Task<PagedResponse<JobResponse>>GetActiveListingsPagedAsync(
                                        JobListingFilterQuery query,
                                                          int page,
                                                       int pageSize);

        Task<bool> ExistsAsync(Guid id);

        Task<bool> IsOpenAsync(Guid id);

        Task AddAsync(JobListing listing);

        Task UpdateAsync(JobListing listing);

        Task CloseAsync(Guid id);
    }
}