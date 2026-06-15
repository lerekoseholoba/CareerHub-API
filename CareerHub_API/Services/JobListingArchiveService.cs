using CareerHub_API.Data;
using Microsoft.EntityFrameworkCore;

namespace CareerHub_API.Services;

public class JobListingArchiveService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobListingArchiveService> _logger;

    public JobListingArchiveService(
        IServiceProvider serviceProvider,
        ILogger<JobListingArchiveService> logger)
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
                        "Archived {Count} expired job listings",
                        expiredJobs.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while archiving expired jobs");
            }

            await Task.Delay(
                TimeSpan.FromHours(1),
                stoppingToken);
        }
    }
}