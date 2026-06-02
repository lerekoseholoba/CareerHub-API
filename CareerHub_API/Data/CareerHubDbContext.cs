using Microsoft.EntityFrameworkCore;
using CareerHub_API.Models; 

namespace CareerHub_API.Data;
public class CareerHubDbContext : DbContext
{
    public CareerHubDbContext(DbContextOptions<CareerHubDbContext> options)
        : base(options) { }

    public DbSet<JobListing> JobListings => Set<JobListing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobListing>(entity =>
        {
            entity.ToTable("job_listings");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Company)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.PostedDate)
                .IsRequired();

            entity.HasIndex(x => new { x.Title, x.Company })
                .IsUnique();
        });
    }
}