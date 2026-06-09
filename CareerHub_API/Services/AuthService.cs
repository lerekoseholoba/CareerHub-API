using CareerHub_API.Data;
using CareerHub_API.Models;
using CareerHub_API.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CareerHub_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly CareerHubDbContext _context;
        private readonly string _jwtSecret;

        public AuthService(CareerHubDbContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtSecret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret missing");
        }

        public async Task<Applicant> RegisterApplicantAsync(string name, string email, string password)
        {
            if (await _context.Applicants.AnyAsync(a => a.Email == email))
                throw new Exception("Email already registered");

            var applicant = new Applicant
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password)
            };

            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();
            return applicant;
        }

        public async Task<string> AuthenticateApplicantAsync(string email, string password)
        {
            var applicant = await _context.Applicants.FirstOrDefaultAsync(a => a.Email == email);
            if (applicant == null || applicant.PasswordHash != HashPassword(password))
                throw new Exception("Invalid credentials");

            // Generate a simple JWT token
            var token = JwtTokenGenerator.GenerateToken(applicant.Id, _jwtSecret);
            return token;
        }

        public Task<Guid?> ValidateTokenAsync(string token)
        {
            var applicantId = JwtTokenGenerator.ValidateToken(token, _jwtSecret);
            return Task.FromResult(applicantId);
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}