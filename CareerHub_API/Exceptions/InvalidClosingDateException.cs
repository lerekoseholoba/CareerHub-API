namespace CareerHub_API.Exceptions;

public class InvalidClosingDateException : Exception
{
    public InvalidClosingDateException()
        : base("The closing date must be in the future.")
    {
    }
}
