namespace KnightShift.Domain.Exceptions;

public sealed class InvalidPositionException : DomainException
{
    public InvalidPositionException(string message) : base(message) { }
}
