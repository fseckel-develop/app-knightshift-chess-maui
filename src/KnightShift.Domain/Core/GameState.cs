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

    public GameState Clone()
    {
        return new GameState
        {
            Board = this.Board.Clone(),
            CurrentTurn = this.CurrentTurn,
            Result = this.Result,
            EndReason = this.EndReason,
            WhiteCanCastleKingSide = this.WhiteCanCastleKingSide,
            WhiteCanCastleQueenSide = this.WhiteCanCastleQueenSide,
            BlackCanCastleKingSide = this.BlackCanCastleKingSide,
            BlackCanCastleQueenSide = this.BlackCanCastleQueenSide,
            EnPassantTarget = this.EnPassantTarget,
            MoveHistory = [.. this.MoveHistory]
        };
    }

    public GameState ApplyMove(Move move)
    {
        var clone = Clone();
        clone.Board.MovePiece(move.From, move.To);
        clone.MoveHistory.Add(move);
        clone.CurrentTurn = clone.CurrentTurn == PieceColor.White
            ? PieceColor.Black
            : PieceColor.White;
        return clone;
    }
}
