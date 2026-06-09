namespace CareerHub_API.Exceptions;

public class ListingClosedException : Exception
{
    public ListingClosedException()
        : base("The job listing is closed and no longer accepting applications.")
    {
    }
}
