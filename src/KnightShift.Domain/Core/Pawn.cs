using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Core;

public static class Pawn
{
    public static int GetForwardDirection(PieceColor color)
        => color == PieceColor.White ? -1 : 1;

    public static int GetStartRank(PieceColor color)
        => color == PieceColor.White ? 2 : 7;

    public static int GetEnPassantCaptureDirection(PieceColor color)
        => color == PieceColor.White ? 1 : -1;

    public static int GetPromotionRank(PieceColor color)
        => color == PieceColor.White ? 8 : 1;

    public static bool IsDoubleMove(int originRow, int targetRow)
        => Math.Abs(originRow - targetRow) == 2;

    public static readonly IEnumerable<int> CaptureOffsets = [-1, 1];

    public static readonly IEnumerable<PieceType> PromotionPieces = 
    [
        PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight
    ];
}
