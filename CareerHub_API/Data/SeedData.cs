using CareerHub_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerHub_API.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var company1 = new Company
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Name = "TechNova",
            Website = "https://technova.com",
            Industry = "Software"
        };

        var company2 = new Company
        {
            Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Name = "FinCore",
            Website = "https://fincore.com",
            Industry = "Finance"
        };

        modelBuilder.Entity<Company>().HasData(company1, company2);

        // IMPORTANT:
        // HasData MUST be deterministic → no DateTime.UtcNow

        modelBuilder.Entity<JobListing>().HasData(
            new JobListing
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Title = "Backend Developer",
                Description = "Seed job",
                Location = "Cape Town",
                CompanyId = company1.Id,

                // FIXED values (deterministic)
                PostedDate = new DateTime(2025, 01, 01, 00, 00, 00, DateTimeKind.Utc),
                ClosingDate = new DateTime(2025, 02, 01, 00, 00, 00, DateTimeKind.Utc),

                IsOpen = true,
                EmploymentType = "Full-Time",
                SalaryMin = 30000,
                SalaryMax = 60000
            }
        );
    }
}