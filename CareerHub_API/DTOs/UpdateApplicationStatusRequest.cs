using CareerHub_API.Models;
namespace CareerHub_API.DTOs;
public class UpdateApplicationStatusRequest
{
    public ApplicationStatus Status { get; set; }
}