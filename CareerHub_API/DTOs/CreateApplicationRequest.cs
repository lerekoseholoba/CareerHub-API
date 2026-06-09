using System.ComponentModel.DataAnnotations;

namespace CareerHub_API.DTOs;

public class CreateApplicationRequest
{
    [Required]
    public Guid JobListingId { get; set; }

    [Required]
    public Guid ApplicantId { get; set; }

    public string? ResumeUrl { get; set; }

    [StringLength(1000)]
    public string? CoverLetter { get; set; }
}
