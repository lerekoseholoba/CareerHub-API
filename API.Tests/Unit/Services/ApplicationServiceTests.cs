using CareerHub_API.DTOs;
using CareerHub_API.Models;
using CareerHub_API.Repositories;
using CareerHub_API.Services;
using NSubstitute;
using Xunit;

namespace API.Tests.Unit.Services;

public class ApplicationServiceTests
{
    private readonly IApplicationRepository _appRepo;
    private readonly IJobListingRepository _jobRepo;
    private readonly ApplicationService _service;

    public ApplicationServiceTests()
    {
        _appRepo = Substitute.For<IApplicationRepository>();
        _jobRepo = Substitute.For<IJobListingRepository>();

        _service = new ApplicationService(
            _appRepo,
            _jobRepo);
    }

    [Theory]
    [InlineData(ApplicationStatus.Submitted, ApplicationStatus.UnderReview)]
    [InlineData(ApplicationStatus.UnderReview, ApplicationStatus.Shortlisted)]
    [InlineData(ApplicationStatus.UnderReview, ApplicationStatus.Rejected)]
    [InlineData(ApplicationStatus.Shortlisted, ApplicationStatus.Offered)]
    [InlineData(ApplicationStatus.Shortlisted, ApplicationStatus.Rejected)]
    public async Task UpdateStatusAsync_WhenTransitionIsLegal_CallsUpdateAsync(
        ApplicationStatus from,
        ApplicationStatus to)
    {
        // Arrange
        var applicantId = Guid.NewGuid();
        var jobId = Guid.NewGuid();

        var application = new Application
        {
            ApplicantId = applicantId,
            JobListingId = jobId,
            Status = from
        };

        _appRepo.GetByIdAsync(applicantId, jobId)
            .Returns(application);

        // Act
        await _service.UpdateStatusAsync(
            applicantId,
            jobId,
            to);

        // Assert
        await _appRepo.Received(1)
            .UpdateStatusAsync(application);
    }

    [Theory]
    [InlineData(ApplicationStatus.Rejected, ApplicationStatus.Submitted)]
    [InlineData(ApplicationStatus.Offered, ApplicationStatus.Submitted)]
    [InlineData(ApplicationStatus.Rejected, ApplicationStatus.UnderReview)]
    [InlineData(ApplicationStatus.Offered, ApplicationStatus.Shortlisted)]
    public async Task UpdateStatusAsync_WhenTransitionIsIllegal_ThrowsInvalidOperationException(
        ApplicationStatus from,
        ApplicationStatus to)
    {
        // Arrange
        var applicantId = Guid.NewGuid();
        var jobId = Guid.NewGuid();

        var application = new Application
        {
            ApplicantId = applicantId,
            JobListingId = jobId,
            Status = from
        };

        _appRepo.GetByIdAsync(applicantId, jobId)
            .Returns(application);

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateStatusAsync(
                applicantId,
                jobId,
                to));

        await _appRepo.DidNotReceive()
            .UpdateStatusAsync(Arg.Any<Application>());
    }

    [Fact]
    public async Task UpdateStatusAsync_WhenApplicationNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var applicantId = Guid.NewGuid();
        var jobId = Guid.NewGuid();

        _appRepo.GetByIdAsync(applicantId, jobId)
            .Returns((Application?)null);

        // Act + Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateStatusAsync(
                applicantId,
                jobId,
                ApplicationStatus.UnderReview));

        await _appRepo.DidNotReceive()
            .UpdateStatusAsync(Arg.Any<Application>());
    }
}