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
        modelBuilder.Entity<Company>(entity =>
       {
             entity.ToTable("companies");

             entity.HasKey(c => c.Id);

             entity.Property(c => c.Name)
                   .HasMaxLength(200)
                   .IsRequired();

             entity.HasIndex(c => c.Name)
                   .IsUnique();
       });
       modelBuilder.Entity<Applicant>(entity =>
       {
             entity.ToTable("applicants");

             entity.HasKey(a => a.Id);

             entity.Property(a => a.Email)
                   .HasMaxLength(255)
                   .IsRequired();

             entity.HasIndex(a => a.Email)
                   .IsUnique();
       });
       modelBuilder.Entity<Application>(entity =>
       {
             entity.ToTable("applications");

             entity.HasKey(a => new
             {
                   a.JobListingId,
                   a.ApplicantId
             });

             entity.HasOne(a => a.JobListing)
                   .WithMany(j => j.Applications)
                   .HasForeignKey(a => a.JobListingId);

             entity.HasOne(a => a.Applicant)
                   .WithMany(a => a.Applications)
                   .HasForeignKey(a => a.ApplicantId);
        });
    }
    private void ConfigureJobListing(ModelBuilder modelBuilder)
    {
    modelBuilder.Entity<JobListing>(entity =>
       {
        entity.HasOne(j => j.Company)
            .WithMany(c => c.JobListings)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}