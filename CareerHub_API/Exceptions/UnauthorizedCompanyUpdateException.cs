namespace CareerHub_API.Exceptions;

public class UnauthorizedCompanyUpdateException : Exception
{
    public UnauthorizedCompanyUpdateException()
        : base("You are not authorized to update this job listing.")
    {
    }
}
