using KnightShift.Domain.Enums;
using KnightShift.Domain.Constants;
using KnightShift.Domain.Exceptions;

namespace KnightShift.Domain.Core;

public class Board
{
    private readonly Piece?[,] _squares = new Piece?[BoardDimensions.Size, BoardDimensions.Size];

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

    public bool IsEmpty(Position position) 
        => GetPiece(position) is null;

    public bool HasEnemyPiece(Position target, PieceColor color)
    {
        var piece = GetPiece(target);
        return piece != null && piece.Color != color;
    }

    public void MovePiece(Position origin, Position target)
    {
        var piece = GetPiece(origin) ?? throw new PieceNotFoundException($"No piece at {origin}");
        SetPiece(target, piece);
        SetPiece(origin, null);
    }

    public IEnumerable<(Position position, Piece piece)> GetAllPieces()
    {
        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            for (int column = 0; column < BoardDimensions.Size; column++)
            {
                var piece = _squares[row, column];
                if (piece is not null)
                {
                    yield return (
                        Position.TryCreateFromCoords(row, column, out var position) 
                            ? position 
                            : throw new InvalidPositionException($"Invalid position: ({row}, {column})"),
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
