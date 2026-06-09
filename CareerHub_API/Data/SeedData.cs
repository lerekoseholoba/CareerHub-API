using CareerHub_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerHub_API.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // =========================
        // Companies
        // =========================

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

        var company5 = new Company
        {
            Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            Name = "GreenEnergy",
            Website = "https://greenenergy.com",
            Industry = "Energy"
        };

        modelBuilder.Entity<Company>().HasData(
            company1,
            company2,
            company3,
            company4,
            company5
        );

        // =========================
        // Job Listings (50)
        // =========================

        var jobs = new List<JobListing>();

        var companies = new[]
        {
            company1.Id,
            company2.Id,
            company3.Id,
            company4.Id,
            company5.Id
        };

        var titles = new[]
        {
            "Backend Developer",
            "Frontend Developer",
            "Full Stack Developer",
            "Cloud Engineer",
            "DevOps Engineer",
            "Software Engineer",
            "Data Analyst",
            "Business Analyst",
            "Financial Analyst",
            "Project Manager"
        };

        var locations = new[]
        {
            "Cape Town",
            "Johannesburg",
            "Pretoria",
            "Durban",
            "Remote"
        };

        var employmentTypes = new[]
        {
            "Full-Time",
            "Part-Time",
            "Contract",
            "Internship"
        };

        for (int i = 1; i <= 50; i++)
        {
            jobs.Add(new JobListing
            {
                Id = Guid.Parse(
                    $"00000000-0000-0000-0000-{i.ToString("D12")}"
                ),

                Title = titles[(i - 1) % titles.Length],

                Description =
                    $"Description for {titles[(i - 1) % titles.Length]} position #{i}",

                Location = locations[(i - 1) % locations.Length],

                CompanyId = companies[(i - 1) % companies.Length],

                PostedDate = new DateTime(
                    2025,
                    1,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc
                ).AddDays(i),

                ClosingDate = new DateTime(
                    2026,
                    12,
                    31,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc
                ),

                IsOpen = true,

                EmploymentType =
                    employmentTypes[(i - 1) % employmentTypes.Length],

                SalaryMin = 20000 + (i * 1000),

                SalaryMax = 30000 + (i * 1000)
            });
        }

        modelBuilder.Entity<JobListing>()
            .HasData(jobs);

        // =========================
        // Applicants
        // =========================

        var applicant1 = new Applicant
        {
            Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111111"),
            Name = "John Doe",
            Email = "john@example.com"
        };

        var applicant2 = new Applicant
        {
            Id = Guid.Parse("bbbbbbbb-2222-2222-2222-222222222222"),
            Name = "Sarah Lee",
            Email = "sarah@example.com"
        };

        var applicant3 = new Applicant
        {
            Id = Guid.Parse("cccccccc-3333-3333-3333-333333333333"),
            Name = "Michael Smith",
            Email = "michael@example.com"
        };

        var applicant4 = new Applicant
        {
            Id = Guid.Parse("dddddddd-4444-4444-4444-444444444444"),
            Name = "Aisha Khan",
            Email = "aisha@example.com"
        };

        var applicant5 = new Applicant
        {
            Id = Guid.Parse("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"),
            Name = "David Brown",
            Email = "david@example.com"
        };

        modelBuilder.Entity<Applicant>().HasData(
            applicant1,
            applicant2,
            applicant3,
            applicant4,
            applicant5
        );

        // =========================
        // Applications
        // =========================

        modelBuilder.Entity<Application>().HasData(

            new Application
            {
                JobListingId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ApplicantId = applicant1.Id,
                SubmittedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = ApplicationStatus.Submitted
            },

            new Application
            {
                JobListingId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ApplicantId = applicant2.Id,
                SubmittedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = ApplicationStatus.UnderReview
            },

            new Application
            {
                JobListingId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                ApplicantId = applicant3.Id,
                SubmittedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = ApplicationStatus.Shortlisted
            },

            new Application
            {
                JobListingId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                ApplicantId = applicant4.Id,
                SubmittedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = ApplicationStatus.Rejected
            },

            new Application
            {
                JobListingId = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                ApplicantId = applicant5.Id,
                SubmittedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = ApplicationStatus.Offered
            }
        );
    }
}