namespace Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;

/// <summary>
/// Thrown when the requested resource not found
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException() : base($"Requested resource not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotFoundException(string resource, Guid id) : base($"{resource} with id {id} not found.")
    {
    }
}