using KnightShift.Domain.Exceptions;

namespace KnightShift.Domain.Core;

public class Board
{
    private readonly Piece?[,] _squares = new Piece?[8, 8];

    public Piece? GetPiece(Position position)
    {
        ValidatePosition(position);
        return _squares[position.ToRow(), position.ToColumn()];
    }

    public void SetPiece(Position position, Piece? piece)
    {
        ValidatePosition(position);
        _squares[position.ToRow(), position.ToColumn()] = piece;
    }

    public bool IsEmpty(Position position) => GetPiece(position) is null;

    public void MovePiece(Position from, Position to)
    {
        var piece = GetPiece(from) ?? throw new PieceNotFoundException($"No piece at {from}");
        SetPiece(to, piece);
        SetPiece(from, null);
    }

    public IEnumerable<(Position position, Piece piece)> GetAllPieces()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int column = 0; column < 8; column++)
            {
                var piece = _squares[row, column];
                if (piece is not null)
                {
                    yield return (
                        Position.FromCoords(row, column),
                        piece
                    );
                }
            }
        }
    }

    public Board Clone()
    {
        var clone = new Board();
        foreach (var (position, piece) in GetAllPieces())
        {
            clone.SetPiece(position, piece);
        }
        return clone;
    }

    private static void ValidatePosition(Position position)
    {
        if (!position.IsValid())
            throw new InvalidPositionException($"Invalid position: {position}");
    }
}
