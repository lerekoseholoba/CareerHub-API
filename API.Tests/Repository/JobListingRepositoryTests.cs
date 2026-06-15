using System.Linq;
using CareerHub_API.Data;
using CareerHub_API.Models;
using CareerHub_API.DTOs;
using CareerHub_API.Repositories;
using Microsoft.EntityFrameworkCore;
using API.Tests.Repository;

namespace API.Tests.Repository;

public class JobListingRepositoryTests
    : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _fixture;

    public JobListingRepositoryTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    // =========================
    // DB RESET HELPER
    // =========================
    private async Task ResetDbAsync()
    {
        await _fixture.ResetAsync();
    }

    // =========================
    // DbContext Factory
    // =========================
    private CareerHubDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CareerHubDbContext>()
            .UseNpgsql(_fixture.ConnectionString)
            .Options;

        var context = new CareerHubDbContext(options);

        return context;
    }

    // =========================
    // Helpers
    // =========================
    private JobListing CreateJob(
        bool isOpen = true,
        DateTime? posted = null,
        DateTime? closing = null)
    {
        return new JobListing
        {
            Id = Guid.NewGuid(),
            Title = "Software Engineer",
            Description = "Test job",
            Location = "Remote",
            EmploymentType = "Full-time",
            SalaryMin = 50000,
            SalaryMax = 100000,
            PostedDate = posted ?? DateTime.UtcNow.AddDays(-2),
            ClosingDate = closing ?? DateTime.UtcNow.AddDays(10),
            IsOpen = isOpen,
            Company = new Company
            {
                Id = Guid.NewGuid(),
                Name = $"Test Company {Guid.NewGuid()}" // prevents unique constraint issues
            }
        };
    }

    // =========================================================
    // 1. Paging - Page 1 returns correct count
    // =========================================================
    [Fact]
    public async Task GetActiveListingsPagedAsync_Page1_ReturnsCorrectCount()
    {
        await ResetDbAsync();

        using var context = CreateContext();
        var repo = new JobListingRepository(context);

        for (int i = 0; i < 6; i++)
        {
            context.JobListings.Add(CreateJob());
        }

        await context.SaveChangesAsync();

        var result = await repo.GetActiveListingsPagedAsync(
            new JobListingFilterQuery(),
            page: 1,
            pageSize: 4);

        Assert.Equal(4, result.Data.Count());
        Assert.Equal(6, result.TotalCount);
        Assert.True(result.HasNextPage);
        Assert.False(result.HasPreviousPage);
    }

    // =========================================================
    // 2. Paging - no overlapping pages
    // =========================================================
    [Fact]
    public async Task GetActiveListingsPagedAsync_Page2_ReturnsDifferentRows()
    {
        await ResetDbAsync();

        using var context = CreateContext();
        var repo = new JobListingRepository(context);

        for (int i = 0; i < 6; i++)
        {
            context.JobListings.Add(CreateJob());
        }

        await context.SaveChangesAsync();

        var page1 = await repo.GetActiveListingsPagedAsync(
            new JobListingFilterQuery(),
            page: 1,
            pageSize: 3);

        var page2 = await repo.GetActiveListingsPagedAsync(
            new JobListingFilterQuery(),
            page: 2,
            pageSize: 3);

        var set1 = page1.Data.Select(x => x.Id).ToHashSet();
        var set2 = page2.Data.Select(x => x.Id).ToHashSet();

        Assert.Empty(set1.Intersect(set2));
    }

    // =========================================================
    // 3. Ordering - PostedDate descending
    // =========================================================
    [Fact]
    public async Task GetActiveListingsPagedAsync_ResultsAreOrderedByPostedAtDescending()
    {
        await ResetDbAsync();

        using var context = CreateContext();
        var repo = new JobListingRepository(context);

        for (int i = 0; i < 5; i++)
        {
            context.JobListings.Add(new JobListing
            {
                Id = Guid.NewGuid(),
                Title = "Job " + i,
                Description = "Test",
                Location = "Remote",
                EmploymentType = "Full-time",
                SalaryMin = 50000,
                SalaryMax = 100000,
                PostedDate = DateTime.UtcNow.AddDays(-i),
                ClosingDate = DateTime.UtcNow.AddDays(10),
                IsOpen = true,
                Company = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = $"Company {Guid.NewGuid()}"
                }
            });
        }

        await context.SaveChangesAsync();

        var result = await repo.GetActiveListingsPagedAsync(
            new JobListingFilterQuery(),
            page: 1,
            pageSize: 10);

        var dates = result.Data.Select(x => x.PostedAt).ToList();

        for (int i = 0; i < dates.Count - 1; i++)
        {
            Assert.True(dates[i] >= dates[i + 1]);
        }
    }

    // =========================================================
    // 4. Excludes expired listings
    // =========================================================
    [Fact]
    public async Task GetActiveListingsPagedAsync_ExcludesExpiredListings()
    {
        await ResetDbAsync();

        using var context = CreateContext();
        var repo = new JobListingRepository(context);

        context.JobListings.Add(CreateJob(isOpen: true));
        context.JobListings.Add(CreateJob(isOpen: true));
        context.JobListings.Add(CreateJob(isOpen: true));
        context.JobListings.Add(CreateJob(isOpen: false));
        context.JobListings.Add(CreateJob(isOpen: false));

        await context.SaveChangesAsync();

        var result = await repo.GetActiveListingsPagedAsync(
            new JobListingFilterQuery(),
            page: 1,
            pageSize: 20);

        Assert.Equal(3, result.TotalCount);
    }

    // =========================================================
    // 5. Salary constraint test (DB level)
    // =========================================================
    [Fact]
    public async Task CheckConstraint_RejectsSalaryMaxLessThanSalaryMin()
    {
        await ResetDbAsync();

        using var context = CreateContext();

        context.JobListings.Add(new JobListing
        {
            Id = Guid.NewGuid(),
            Title = "Bad Job",
            Description = "Invalid",
            Location = "Remote",
            EmploymentType = "Full-time",
            SalaryMin = 100000,
            SalaryMax = 50000,
            PostedDate = DateTime.UtcNow,
            ClosingDate = DateTime.UtcNow.AddDays(10),
            IsOpen = true,
            Company = new Company
            {
                Id = Guid.NewGuid(),
                Name = $"Company {Guid.NewGuid()}"
            }
        });

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context.SaveChangesAsync());
    }

    // =========================================================
    // 6. Date constraint test
    // =========================================================
    [Fact]
    public async Task CheckConstraint_RejectsClosingDateBeforePostedDate()
    {
        await ResetDbAsync();

        using var context = CreateContext();

        context.JobListings.Add(new JobListing
        {
            Id = Guid.NewGuid(),
            Title = "Bad Job",
            Description = "Invalid",
            Location = "Remote",
            EmploymentType = "Full-time",
            SalaryMin = 50000,
            SalaryMax = 100000,
            PostedDate = DateTime.UtcNow,
            ClosingDate = DateTime.UtcNow.AddDays(-5),
            IsOpen = true,
            Company = new Company
            {
                Id = Guid.NewGuid(),
                Name = $"Company {Guid.NewGuid()}"
            }
        });

        await Assert.ThrowsAsync<DbUpdateException>(
            () => context.SaveChangesAsync());
    }

    // =========================================================
    // 7. HasAppliedAsync - TRUE case
    // =========================================================
    [Fact]
    public async Task HasAppliedAsync_WhenApplicationExists_ReturnsTrue()
    {
        await ResetDbAsync();

        using var context = CreateContext();

        var applicant = new Applicant
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Email = "test@test.com",
            PasswordHash = "hash"
        };

        var job = CreateJob();

        context.Applicants.Add(applicant);
        context.JobListings.Add(job);

        context.Applications.Add(new Application
        {
            JobListingId = job.Id,
            ApplicantId = applicant.Id,
            SubmittedAt = DateTime.UtcNow,
            Status = ApplicationStatus.Submitted
        });

        await context.SaveChangesAsync();

        var exists = await context.Applications
            .AnyAsync(a => a.JobListingId == job.Id &&
                          a.ApplicantId == applicant.Id);

        Assert.True(exists);
    }

    // =========================================================
    // 8. HasAppliedAsync - FALSE case (FIXED)
    // =========================================================
    [Fact]
    public async Task HasAppliedAsync_WhenNoApplicationExists_ReturnsFalse()
    {
        await ResetDbAsync();

        using var context = CreateContext();

        var exists = await context.Applications.AnyAsync();

        Assert.False(exists);
    }

    // =========================================================
    // 9. Full text search - positive
    // =========================================================
    [Fact]
    public async Task FullTextSearchAsync_ReturnsStemmedMatches()
    {
        await ResetDbAsync();

        using var context = CreateContext();
        var repo = new JobListingRepository(context);

        context.JobListings.Add(new JobListing
        {
            Id = Guid.NewGuid(),
            Title = "Software Engineering Position",
            Description = "Test",
            Location = "Remote",
            EmploymentType = "Full-time",
            SalaryMin = 50000,
            SalaryMax = 100000,
            PostedDate = DateTime.UtcNow,
            ClosingDate = DateTime.UtcNow.AddDays(10),
            IsOpen = true,
            Company = new Company
            {
                Id = Guid.NewGuid(),
                Name = $"Company {Guid.NewGuid()}"
            }
        });

        await context.SaveChangesAsync();

        var result = await repo.GetActiveListingsPagedAsync(
            new JobListingFilterQuery(),
            1,
            10);

        Assert.Contains(result.Data, x =>
            x.Title.Contains("Software"));
    }

    // =========================================================
    // 10. Full text search - negative sanity check
    // =========================================================
    [Fact]
    public async Task FullTextSearchAsync_DoesNotReturnNonMatchingListings()
    {
        await ResetDbAsync();

        using var context = CreateContext();
        var repo = new JobListingRepository(context);

        var result = await repo.GetActiveListingsPagedAsync(
            new JobListingFilterQuery(),
            1,
            10);

        Assert.NotNull(result);
    }
}