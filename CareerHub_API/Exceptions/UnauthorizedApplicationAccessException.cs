namespace CareerHub_API.Exceptions;

public class UnauthorizedApplicationAccessException : Exception
{
    public UnauthorizedApplicationAccessException()
        : base("You are not authorized to access this application.")
    {
    }
}
