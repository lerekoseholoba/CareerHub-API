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

        var company3 = new Company
        {
            Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            Name = "HealthPlus",
            Website = "https://healthplus.com",
            Industry = "Healthcare"
        };

        var company4 = new Company
        {
            Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            Name = "EduSmart",
            Website = "https://edusmart.com",
            Industry = "Education"
        };

        modelBuilder.Entity<Company>().HasData(company1, company2, company3, company4);

       modelBuilder.Entity<JobListing>().HasData(

    new JobListing
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Title = "Backend Developer",
        Description = "ASP.NET Core backend development role",
        Location = "Cape Town",
        CompanyId = company1.Id,
        PostedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        ClosingDate = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsOpen = true,
        EmploymentType = JobType.FullTime,
        SalaryMin = 30000,
        SalaryMax = 60000
    },

    new JobListing
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Title = "Frontend Developer",
        Description = "React developer role",
        Location = "Johannesburg",
        CompanyId = company1.Id,
        PostedDate = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc),
        ClosingDate = new DateTime(2027, 1, 5, 0, 0, 0, DateTimeKind.Utc),
        IsOpen = true,
        EmploymentType = JobType.FullTime,
        SalaryMin = 25000,
        SalaryMax = 55000
    },

    new JobListing
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
        Title = "Data Analyst",
        Description = "Finance data analysis role",
        Location = "Pretoria",
        CompanyId = company2.Id,
        PostedDate = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc),
        ClosingDate = new DateTime(2027, 1, 10, 0, 0, 0, DateTimeKind.Utc),
        IsOpen = true,
        EmploymentType = JobType.Contract,
        SalaryMin = 20000,
        SalaryMax = 45000
    },

    new JobListing
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
        Title = "Healthcare Systems Engineer",
        Description = "Build healthcare technology solutions",
        Location = "Durban",
        CompanyId = company3.Id,
        PostedDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
        ClosingDate = new DateTime(2027, 1, 15, 0, 0, 0, DateTimeKind.Utc),
        IsOpen = true,
        EmploymentType = JobType.FullTime,
        SalaryMin = 35000,
        SalaryMax = 70000
    }
);}
}