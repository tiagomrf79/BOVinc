namespace Animal.API.Exceptions;

public class AnimalException : Exception
{
    public AnimalException()
    { }

    public AnimalException(string message)
        : base (message)
    { }

    public AnimalException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
