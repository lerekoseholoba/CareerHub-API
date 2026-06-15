using CareerHub_API.Data;
using Microsoft.EntityFrameworkCore;
using CareerHub_API.Models;
using CareerHub_API.DTOs;
namespace CareerHub_API.Services;
public class JobListingExpiryService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobListingExpiryService> _logger;

    public JobListingExpiryService(
        IServiceProvider serviceProvider,
        ILogger<JobListingExpiryService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope =
                    _serviceProvider.CreateScope();

                var context =
                    scope.ServiceProvider
                        .GetRequiredService<CareerHubDbContext>();

                var expiredJobs =
                    await context.JobListings
                        .Where(j =>
                            j.IsOpen &&
                            j.ClosingDate < DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                if (expiredJobs.Count > 0)
                {
                    foreach (var job in expiredJobs)
                    {
                        job.IsOpen = false;
                    }

                    await context.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation(
                        "Expired {Count} job listings",
                        expiredJobs.Count);
                }
                else
                {
                    _logger.LogInformation("No job listings to expire.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while expiring jobs");
            }

            await Task.Delay(
                TimeSpan.FromHours(24),
                stoppingToken);
        }
    }
}
/*
public class JobListingExpiryService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobListingExpiryService> _logger;

    public JobListingExpiryService(
        IServiceProvider serviceProvider,
        ILogger<JobListingExpiryService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope =
                    _serviceProvider.CreateScope();

                var context =
                    scope.ServiceProvider
                        .GetRequiredService<CareerHubDbContext>();

                var expiredJobs =
                    await context.JobListings
                        .Where(j =>
                            j.IsOpen &&
                            j.ClosingDate < DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                if (expiredJobs.Count > 0)
                {
                    foreach (var job in expiredJobs)
                    {
                        job.IsOpen = false;
                    }

                    await context.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation(
                        "Expired {Count} job listings",
                        expiredJobs.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while expiring jobs");
            }

            await Task.Delay(
                TimeSpan.FromHours(1),
                stoppingToken);
        }
    }
}
*/