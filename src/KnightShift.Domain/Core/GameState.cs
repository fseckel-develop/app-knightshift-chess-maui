using KnightShift.Domain.Enums;
using KnightShift.Domain.Exceptions;

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

        HandleCastling(clone, move);
        HandleEnPassant(clone, move);
        UpdateEnPassantTarget(clone, move);

        clone.Board.MovePiece(move.From, move.To);
        clone.MoveHistory.Add(move);
        clone.SwitchTurn();

        return clone;
    }

    private static void HandleCastling(GameState state, Move move)
    {
        if (!move.IsCastling)
            return;

        bool isKingSide = move.To.ToColumn() == 6;
        var rookFromColumn = isKingSide ? 7 : 0;
        var rookToColumn = isKingSide ? 5 : 3;

        int row = move.From.ToRow();
        var rookFrom = Position.CreateFromCoords(row, rookFromColumn);
        var rookTo = Position.CreateFromCoords(row, rookToColumn);

        var rook = state.Board.GetPiece(rookFrom);
        if (rook is null || rook.Type != PieceType.Rook || rook.Color != state.CurrentTurn)
        {
            throw new InvalidBoardOperationException("Invalid castling state.");
        }

        state.Board.SetPiece(rookTo, rook);
        state.Board.SetPiece(rookFrom, null);
    }

    private static void HandleEnPassant(GameState state, Move move)
    {
        if (!move.IsEnPassant)
            return;

        int direction = state.CurrentTurn == PieceColor.White ? 1 : -1;

        var capturedRow = move.To.ToRow() + direction;
        var capturedColumn = move.To.ToColumn();
        var capturedPosition = Position.CreateFromCoords(capturedRow, capturedColumn);

        state.Board.SetPiece(capturedPosition, null);
    }

    private static void UpdateEnPassantTarget(GameState state, Move move)
    {
        var movingPiece = state.Board.GetPiece(move.From)
            ?? throw new InvalidBoardOperationException("No piece at source.");
    
        if (movingPiece?.Type != PieceType.Pawn)
        {
            state.EnPassantTarget = null;
            return;
        }

        int fromRow = move.From.ToRow();
        int toRow = move.To.ToRow();

        if (Math.Abs(fromRow - toRow) == 2)
        {
            int middleRow = (fromRow + toRow) / 2;
            int column = move.From.ToColumn();

            state.EnPassantTarget = Position.CreateFromCoords(middleRow, column);
        }
        else
        {
            state.EnPassantTarget = null;
        }
    }

    private void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == PieceColor.White
            ? PieceColor.Black 
            : PieceColor.White;
    }
}
