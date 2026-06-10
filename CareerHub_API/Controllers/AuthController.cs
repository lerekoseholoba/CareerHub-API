using CareerHub_API.DTOs;
using CareerHub_API.Services;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace CareerHub_API.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.Password != request.Password)
                return BadRequest(new { message = "Passwords do not match" });

            var applicant = await _authService.RegisterApplicantAsync(request.Name, request.Email, request.Password);

            return Ok(new { message = "Registration successful", applicantId = applicant.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.AuthenticateApplicantAsync(request.Email, request.Password);

                return Ok(new
                {
                    token = token,
                    message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
