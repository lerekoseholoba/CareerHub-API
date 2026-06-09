using System.ComponentModel.DataAnnotations;

namespace CareerHub_API.DTOs;

public class CreateApplicationRequest
{
    [Required]
    public Guid ApplicantId { get; set; }

    [Required]
    public Guid JobListingId { get; set; }

    [Required]
    [Url]
    public string ResumeUrl { get; set; } = string.Empty;

    [StringLength(1000)]
    public string CoverLetter { get; set; } = string.Empty;
}
