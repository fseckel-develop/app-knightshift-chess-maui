namespace KnightShift.Domain.Exceptions;

public sealed class InvalidMoveException : DomainException
{
    public InvalidMoveException(string message) : base(message) { }
}
