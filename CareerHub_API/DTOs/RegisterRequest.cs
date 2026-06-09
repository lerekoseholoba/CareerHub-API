namespace CareerHub_API.DTOs;

public record RegisterRequest(string Name, string Email, string Password, string ConfirmPassword);
