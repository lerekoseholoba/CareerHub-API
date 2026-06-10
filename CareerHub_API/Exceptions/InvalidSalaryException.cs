namespace CareerHub_API.Exceptions;

public class InvalidSalaryException : Exception
{
    public InvalidSalaryException()
        : base("SalaryMax must be greater than SalaryMin.")
    {
    }
}