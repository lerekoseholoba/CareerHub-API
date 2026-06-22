using CareerHub_API.Models;

namespace CareerHub_API.DTOs;

public class UpdateJobListingRequest
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public JobType? EmploymentType { get; set; }   // ✅ CHANGED

    public decimal? SalaryMin { get; set; }

    public decimal? SalaryMax { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
/*
namespace CareerHub_API.DTOs;
public class UpdateJobListingRequest
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? EmploymentType { get; set; }

    public decimal? SalaryMin { get; set; }

    public decimal? SalaryMax { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
*/