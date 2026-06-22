using CareerHub_API.Models;

namespace CareerHub_API.DTOs;

public class JobResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime PostedAt { get; set; }

    public JobType EmploymentType { get; set; }   // ✅ CHANGED

    public decimal SalaryMin { get; set; }

    public decimal SalaryMax { get; set; }

    public bool IsOpen { get; set; }

    public int ApplicationCount { get; set; }
}
/*
using CareerHub_API.Models;

namespace CareerHub_API.DTOs;

public class JobResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime PostedAt { get; set; }

    public string EmploymentType { get; set; } = string.Empty;

    public decimal SalaryMin { get; set; }

    public decimal SalaryMax { get; set; }

    public bool IsOpen { get; set; }

    public int ApplicationCount { get; set; }
}
*/