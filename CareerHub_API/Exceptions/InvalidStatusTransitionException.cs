namespace CareerHub_API.Exceptions;

public class InvalidStatusTransitionException : Exception
{
    public InvalidStatusTransitionException(object from, object to)
        : base($"Cannot transition from {from} to {to}.")
    {
    }
}
