namespace CareerHub_API.Exceptions;

public class DuplicateApplicationException : Exception
{
    public DuplicateApplicationException()
        : base("You have already applied for this job.")
    {
    }
}
