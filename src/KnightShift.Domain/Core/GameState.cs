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
        UpdateCastlingRights(clone, move);
        UpdateEnPassantTarget(clone, move);

        clone.Board.MovePiece(move.Origin, move.Target);
        HandlePromotion(clone, move);
        clone.MoveHistory.Add(move);
        clone.SwitchTurn();

        return clone;
    }

    private static void HandleCastling(GameState state, Move move)
    {
        if (!move.IsCastling)
            return;

        var side = move.Target.ToColumn() == Castling.KingSide.KingTargetColumn
            ? CastlingSide.KingSide
            : CastlingSide.QueenSide;

        var rookOrigin = Castling.GetRookOrigin(state.CurrentTurn, side);
        var rookTarget = Castling.GetRookTarget(state.CurrentTurn, side);

        var rook = state.Board.GetPiece(rookOrigin);
        if (rook is null || rook.Type != PieceType.Rook || rook.Color != state.CurrentTurn)
        {
            throw new InvalidBoardOperationException("Invalid castling state.");
        }

        state.Board.SetPiece(rookTarget, rook);
        state.Board.SetPiece(rookOrigin, null);
    }

    private static void UpdateCastlingRights(GameState state, Move move)
    {
        var movingPiece = state.Board.GetPiece(move.Origin)
            ?? throw new InvalidBoardOperationException("No piece at source.");
            
        var capturedPiece = state.Board.GetPiece(move.Target);

        if (movingPiece.Type == PieceType.King)
        {
            if (movingPiece.Color == PieceColor.White)
            {
                state.WhiteCanCastleKingSide = false;
                state.WhiteCanCastleQueenSide = false;
            }
            else
            {
                state.BlackCanCastleKingSide = false;
                state.BlackCanCastleQueenSide = false;
            }
        }

        if (movingPiece.Type == PieceType.Rook)
        {
            DisableCastlingRightForRook(state, movingPiece.Color, move.Origin);
        }
        
        if (capturedPiece?.Type == PieceType.Rook)
        {
            DisableCastlingRightForRook(state, capturedPiece.Color, move.Target);
        }
    }

    private static void DisableCastlingRightForRook(GameState state, PieceColor color, Position position)
    {
        if (position == Castling.GetRookOrigin(color, CastlingSide.KingSide))
        {
            if (color == PieceColor.White)
                state.WhiteCanCastleKingSide = false;
            else
                state.BlackCanCastleKingSide = false;
        }

        if (position == Castling.GetRookOrigin(color, CastlingSide.QueenSide))
        {
            if (color == PieceColor.White)
                state.WhiteCanCastleQueenSide = false;
            else
                state.BlackCanCastleQueenSide = false;
        }
    }

    private static void HandleEnPassant(GameState state, Move move)
    {
        if (!move.IsEnPassant)
            return;

        int direction = Pawn.GetEnPassantCaptureDirection(state.CurrentTurn);

        var capturedRow = move.Target.ToRow() + direction;
        var capturedColumn = move.Target.ToColumn();
        var capturedPosition = Position.CreateFromCoords(capturedRow, capturedColumn);

        state.Board.SetPiece(capturedPosition, null);
    }

    private static void UpdateEnPassantTarget(GameState state, Move move)
    {
        var movingPiece = state.Board.GetPiece(move.Origin)
            ?? throw new InvalidBoardOperationException("No piece at source.");
    
        if (movingPiece?.Type != PieceType.Pawn)
        {
            state.EnPassantTarget = null;
            return;
        }

        int originRow = move.Origin.ToRow();
        int targetRow = move.Target.ToRow();

        if (Pawn.IsDoubleMove(originRow, targetRow))
        {
            int middleRow = (originRow + targetRow) / 2;
            int column = move.Origin.ToColumn();
            state.EnPassantTarget = Position.CreateFromCoords(middleRow, column);
        }
        else
        {
            state.EnPassantTarget = null;
        }
    }

    private static void HandlePromotion(GameState state, Move move)
    {
        if (move.Promotion is null)
            return;

        var piece = state.Board.GetPiece(move.Target);
        if (piece?.Type != PieceType.Pawn)
        {
            throw new InvalidBoardOperationException("Invalid promotion: no pawn at target.");
        }

        var promotedPiece = new Piece(move.Promotion.Value, piece.Color);
        state.Board.SetPiece(move.Target, promotedPiece);
    }

    private void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == PieceColor.White
            ? PieceColor.Black 
            : PieceColor.White;
    }
}
