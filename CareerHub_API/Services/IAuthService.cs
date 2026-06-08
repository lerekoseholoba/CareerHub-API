using CareerHub_API.Models;

namespace CareerHub_API.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates an applicant and returns a JWT token
        /// </summary>
        Task<string> AuthenticateApplicantAsync(string email, string password);

        /// <summary>
        /// Validates a token and returns the applicant's ID if valid
        /// </summary>
        Task<Guid?> ValidateTokenAsync(string token);

        /// <summary>
        /// Registers a new applicant
        /// </summary>
        Task<Applicant> RegisterApplicantAsync(string name, string email, string password);
    }
}