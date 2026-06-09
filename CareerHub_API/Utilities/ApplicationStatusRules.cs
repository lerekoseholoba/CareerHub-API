using CareerHub_API.Models;

namespace CareerHub_API.Utilities;

public static class ApplicationStatusRules
{
    private static readonly Dictionary<ApplicationStatus, ApplicationStatus[]> ValidTransitions = new()
    {
        { ApplicationStatus.Submitted, new[] { ApplicationStatus.UnderReview } },
        { ApplicationStatus.UnderReview, new[] { ApplicationStatus.Shortlisted, ApplicationStatus.Rejected } },
        { ApplicationStatus.Shortlisted, new[] { ApplicationStatus.Offered, ApplicationStatus.Rejected } },
        { ApplicationStatus.Offered, Array.Empty<ApplicationStatus>() },
        { ApplicationStatus.Rejected, Array.Empty<ApplicationStatus>() },
        { ApplicationStatus.Withdrawn, Array.Empty<ApplicationStatus>() }
    };

    public static bool IsValidTransition(ApplicationStatus from, ApplicationStatus to)
    {
        if (!ValidTransitions.TryGetValue(from, out var allowed))
            return false;

        return allowed.Contains(to);
    }
}
