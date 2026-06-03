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

        var company5 = new Company
        {
            Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            Name = "GreenEnergy",
            Website = "https://greenenergy.com",
            Industry = "Energy"
        };

        modelBuilder.Entity<Company>().HasData(company1, company2, company3, company4, company5);

        var job1 = new JobListing
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Title = "Backend Developer",
            Description = "Build APIs with .NET",
            Location = "Remote",
            CompanyId = company1.Id,
            PostedDate = DateTime.UtcNow
        };

        var job2 = new JobListing
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Title = "Financial Analyst",
            Description = "Analyse financial data",
            Location = "Johannesburg",
            CompanyId = company2.Id,
            PostedDate = DateTime.UtcNow
        };

        var job3 = new JobListing
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Title = "Nurse Specialist",
            Description = "Patient care and support",
            Location = "Cape Town",
            CompanyId = company3.Id,
            PostedDate = DateTime.UtcNow
        };

        var job4 = new JobListing
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Title = "Lecturer",
            Description = "Teach computer science",
            Location = "Pretoria",
            CompanyId = company4.Id,
            PostedDate = DateTime.UtcNow
        };

        var job5 = new JobListing
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Title = "Energy Engineer",
            Description = "Renewable energy systems",
            Location = "Durban",
            CompanyId = company5.Id,
            PostedDate = DateTime.UtcNow
        };

        modelBuilder.Entity<JobListing>().HasData(job1, job2, job3, job4, job5);

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
            Id = Guid.Parse("eeeeeeee-5555-5555-5555-555555555555"),
            Name = "David Brown",
            Email = "david@example.com"
        };

        modelBuilder.Entity<Applicant>().HasData(applicant1, applicant2, applicant3, applicant4, applicant5);

        modelBuilder.Entity<Application>().HasData(
            new Application
            {
                JobListingId = job1.Id,
                ApplicantId = applicant1.Id,
                SubmittedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Submitted
            },
            new Application
            {
                JobListingId = job2.Id,
                ApplicantId = applicant2.Id,
                SubmittedAt = DateTime.UtcNow,
                Status = ApplicationStatus.UnderReview
            },
            new Application
            {
                JobListingId = job3.Id,
                ApplicantId = applicant3.Id,
                SubmittedAt = DateTime.UtcNow,
                Status = ApplicationStatus.InterviewScheduled
            },
            new Application
            {
                JobListingId = job4.Id,
                ApplicantId = applicant4.Id,
                SubmittedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Rejected
            },
            new Application
            {
                JobListingId = job5.Id,
                ApplicantId = applicant5.Id,
                SubmittedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Accepted
            }
        );
    }
}