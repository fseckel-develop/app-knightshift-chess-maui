using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Core;

public sealed record CastlingData(
    int KingTargetColumn,
    int RookOriginColumn,
    int RookTargetColumn,
    int[] PathColumns
);

public static class Castling
{
    public static readonly CastlingData KingSide = new(
        KingTargetColumn: 6,
        RookOriginColumn: 7,
        RookTargetColumn: 5,
        PathColumns: [5, 6]
    );

    public static readonly CastlingData QueenSide = new(
        KingTargetColumn: 2,
        RookOriginColumn: 0,
        RookTargetColumn: 3,
        PathColumns: [3, 2, 1]
    );

    private static CastlingData FromSide(CastlingSide side)
        => side == CastlingSide.KingSide ? KingSide : QueenSide;

    private static int GetBackRankRow(PieceColor color)
        => color == PieceColor.White ? 7 : 0;

    public static Position GetKingTarget(PieceColor color, CastlingSide side)
    {
        int row = GetBackRankRow(color);
        var castling = FromSide(side);
        return Position.CreateFromCoords(row, castling.KingTargetColumn);
    }

    public static Position GetRookOrigin(PieceColor color, CastlingSide side)
    {
        int row = GetBackRankRow(color);
        var castling = FromSide(side);
        return Position.CreateFromCoords(row, castling.RookOriginColumn);
    }

    public static Position GetRookTarget(PieceColor color, CastlingSide side)
    {
        int row = GetBackRankRow(color);
        var castling = FromSide(side);
        return Position.CreateFromCoords(row, castling.RookTargetColumn);
    }

    public static IEnumerable<Position> GetPathSquares(PieceColor color, CastlingSide side)
    {
        int row = GetBackRankRow(color);
        var castling = FromSide(side);
        return castling.PathColumns.Select(column => Position.CreateFromCoords(row, column));
    }
}
