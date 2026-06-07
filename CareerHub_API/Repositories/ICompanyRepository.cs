using CareerHub_API.Models;

namespace CareerHub_API.Repositories
{
    public interface ICompanyRepository
    {
        Task<bool> ExistsAsync(Guid companyId);

        Task<Company?> GetByIdAsync(Guid companyId);
    }
}