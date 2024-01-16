namespace Prohelika.Template.CleanArchitecture.Application.Common.Exceptions;

/// <summary>
/// Thrown when the user isn't authorized to perform the action
/// </summary>
public class ForbiddenException : Exception

{
    public ForbiddenException() : base("You are not allowed to perform this operation")
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}