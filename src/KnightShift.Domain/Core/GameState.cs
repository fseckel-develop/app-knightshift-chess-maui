using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Core;

public sealed class GameState
{
    public Board Board { get; init; } = new();
    public PieceColor CurrentTurn { get; set; } = PieceColor.White;
    public GameResult Result { get; set; } = GameResult.Ongoing;
    public GameEndReason EndReason { get; set; } = GameEndReason.None;
    public bool WhiteCanCastleKingSide { get; set; } = true;
    public bool WhiteCanCastleQueenSide { get; set; } = true;
    public bool BlackCanCastleKingSide { get; set; } = true;
    public bool BlackCanCastleQueenSide { get; set; } = true;
    public Position? EnPassantTarget { get; set; } = null;
    public List<Move> MoveHistory { get; init; } = [];

    public void ApplyMove(Move move)
    {
        Board.MovePiece(move.From, move.To);
        MoveHistory.Add(move);
        CurrentTurn = CurrentTurn == PieceColor.White
            ? PieceColor.Black
            : PieceColor.White;
    }
}
