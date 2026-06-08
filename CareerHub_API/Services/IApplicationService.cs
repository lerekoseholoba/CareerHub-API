namespace CareerHub_API.Services
{
    public interface IApplicationService
    {
        Task SubmitApplicationAsync(CreateApplicationRequest request);
        Task UpdateStatusAsync(Guid applicantId, Guid jobId, ApplicationStatus status);
        Task WithdrawAsync(Guid applicantId, Guid jobId, Guid currentUserApplicantId);
    }
}