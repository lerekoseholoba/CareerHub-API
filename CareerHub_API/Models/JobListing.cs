public class JobListing
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    public ICollection<Application> Applications { get; set; }
        = new List<Application>();

    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;
}