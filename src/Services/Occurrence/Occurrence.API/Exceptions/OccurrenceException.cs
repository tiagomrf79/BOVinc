namespace Occurrence.API.Exceptions;

internal class OccurrenceException : Exception
{
    public OccurrenceException()
    { }

    public OccurrenceException(string message)
        : base(message)
    { }

    public OccurrenceException(string message, Exception innerException)
        : base(message, innerException)
    { }

}