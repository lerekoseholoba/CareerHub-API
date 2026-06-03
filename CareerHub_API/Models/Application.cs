public class Application
{
    public Guid JobListingId { get; set; }

    public Guid ApplicantId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public ApplicationStatus Status { get; set; }

    public JobListing JobListing { get; set; } = null!;

    public Applicant Applicant { get; set; } = null!;
}