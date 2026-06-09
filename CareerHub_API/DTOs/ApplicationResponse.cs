using CareerHub_API.Models;

namespace CareerHub_API.DTOs;

public class ApplicationResponse
{
    public Guid ApplicantId { get; set; }

    public Guid JobListingId { get; set; }

    public string ApplicantName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public ApplicationStatus Status { get; set; }

    public DateTime SubmittedAt { get; set; }
}