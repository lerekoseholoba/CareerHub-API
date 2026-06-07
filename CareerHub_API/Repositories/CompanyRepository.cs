using CareerHub_API.Data;
using CareerHub_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerHub_API.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CareerHubDbContext _context;

        public CompanyRepository(CareerHubDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Guid companyId)
        {
            return await _context.Companies.AnyAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetByIdAsync(Guid companyId)
        {
            return await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }
    }
}