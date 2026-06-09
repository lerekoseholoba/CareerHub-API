namespace CareerHub_API.Models;

public class JobListing
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    public ICollection<Application> Applications { get; set; }
        = new List<Application>();

    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime PostedDate { get; set; }

    public DateTime ClosingDate { get; set; }

    public bool IsOpen { get; set; }

    public string EmploymentType { get; set; } = string.Empty;

    public decimal SalaryMin { get; set; }

    public decimal SalaryMax { get; set; }
}