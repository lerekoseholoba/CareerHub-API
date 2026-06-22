using System.ComponentModel.DataAnnotations;
using CareerHub_API.Models;

namespace CareerHub_API.DTOs;
public class CreateJobRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
    public string Location { get; set; } = string.Empty;

    public DateTime ClosingDate { get; set; }

    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }

    // ✅ ADD THIS
    public JobType EmploymentType { get; set; } 
}
/*
public class CreateJobRequest : IValidatableObject
{
    [Required]
    [StringLength(120, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public Guid CompanyId { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MinLength(20)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime ClosingDate { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? SalaryMin { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? SalaryMax { get; set; }

    // Cross-field validation
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ClosingDate <= DateTime.UtcNow)
        {
            yield return new ValidationResult(
                "ClosingDate must be in the future",
                new[] { nameof(ClosingDate) });
        }

        if (SalaryMin.HasValue &&
            SalaryMax.HasValue &&
            SalaryMax <= SalaryMin)
        {
            yield return new ValidationResult(
                "SalaryMax must be greater than SalaryMin",
                new[] { nameof(SalaryMax) });
        }
    }
    
}
*/
