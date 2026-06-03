using Microsoft.EntityFrameworkCore;
using CareerHub_API.Models;

namespace CareerHub_API.Data;

public class CareerHubDbContext : DbContext
{
    public CareerHubDbContext(DbContextOptions<CareerHubDbContext> options)
        : base(options) { }

    public DbSet<JobListing> JobListings => Set<JobListing>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobListing>(entity =>
        {
            entity.ToTable("job_listings");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
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

            entity.HasIndex(x => x.Title);

            entity.HasOne(x => x.Company)
                .WithMany(c => c.JobListings)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("companies");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(c => c.Website)
                .HasMaxLength(200);

            entity.Property(c => c.Industry)
                .HasMaxLength(100);

            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.ToTable("applicants");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(a => a.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(a => a.Email)
                .IsUnique();
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("applications");

            entity.HasKey(a => new { a.JobListingId, a.ApplicantId });

            entity.Property(a => a.SubmittedAt)
                .IsRequired();

            entity.Property(a => a.Status)
                .IsRequired();

            entity.HasOne(a => a.JobListing)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobListingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.Applicant)
                .WithMany(a => a.Applications)
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}