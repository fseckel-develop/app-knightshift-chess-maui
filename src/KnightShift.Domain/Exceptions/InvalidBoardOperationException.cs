namespace KnightShift.Domain.Exceptions;

public sealed class InvalidBoardOperationException : DomainException
{
    public InvalidBoardOperationException(string message) : base(message) { }
}
