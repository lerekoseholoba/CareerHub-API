namespace CareerHub_Api.Models;

public class JobListing
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public JobType Type { get; set; }

    public decimal? SalaryMin { get; set; }

    public decimal? SalaryMax { get; set; }

    // Server-owned fields
    public DateTime PostedAt { get; set; }

    public bool IsActive { get; set; }
}