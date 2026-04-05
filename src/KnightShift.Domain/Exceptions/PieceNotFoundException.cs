namespace KnightShift.Domain.Exceptions;

public sealed class PieceNotFoundException : DomainException
{
    public PieceNotFoundException(string message) : base(message) { }
}
