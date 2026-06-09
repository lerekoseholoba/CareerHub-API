using CareerHub_API.Models;

namespace CareerHub_API.Models;

public class Application
{
    public Guid JobListingId { get; set; }
    public Guid ApplicantId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public ApplicationStatus Status { get; set; }
    public string ResumeUrl { get; set; } = string.Empty;
    public string CoverLetter { get; set; } = string.Empty;

    public JobListing JobListing { get; set; } = null!;
    public Applicant Applicant { get; set; } = null!;

    private static readonly Dictionary<ApplicationStatus, ApplicationStatus[]> ValidTransitions = new()
    {
        { ApplicationStatus.Submitted, new[] { ApplicationStatus.UnderReview } },
        { ApplicationStatus.UnderReview, new[] { ApplicationStatus.InterviewScheduled, ApplicationStatus.Rejected } },
        { ApplicationStatus.InterviewScheduled, new[] { ApplicationStatus.Accepted, ApplicationStatus.Rejected } },
        { ApplicationStatus.Accepted, Array.Empty<ApplicationStatus>() },
        { ApplicationStatus.Rejected, Array.Empty<ApplicationStatus>() }
    };

    public void TransitionTo(ApplicationStatus newStatus)
    {
        if (!ValidTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatus))
            throw new InvalidOperationException(
                $"Invalid status transition from {Status} to {newStatus}");

        Status = newStatus;
    }
}
