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

        clone.Board.MovePiece(move.From, move.To);
        HandlePromotion(clone, move);
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

    private static void UpdateCastlingRights(GameState state, Move move)
    {
        var movingPiece = state.Board.GetPiece(move.From)
            ?? throw new InvalidBoardOperationException("No piece at source.");
            
        var capturedPiece = state.Board.GetPiece(move.To);

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
            DisableCastlingRightForRook(state, movingPiece.Color, move.From);
        }
        
        if (capturedPiece?.Type == PieceType.Rook)
        {
            DisableCastlingRightForRook(state, capturedPiece.Color, move.To);
        }
    }

    private static void DisableCastlingRightForRook(GameState state, PieceColor color, Position position)
    {
        if (color == PieceColor.White)
        {
            if (position.Rank == 1 && position.File == 'h')
                state.WhiteCanCastleKingSide = false;

            if (position.Rank == 1 && position.File == 'a')
                state.WhiteCanCastleQueenSide = false;
        }
        else
        {
            if (position.Rank == 8 && position.File == 'h')
                state.BlackCanCastleKingSide = false;

            if (position.Rank == 8 && position.File == 'a')
                state.BlackCanCastleQueenSide = false;
        }
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

    private static void HandlePromotion(GameState state, Move move)
    {
        if (move.Promotion is null)
            return;

        var piece = state.Board.GetPiece(move.To);
        if (piece?.Type != PieceType.Pawn)
        {
            throw new InvalidBoardOperationException("Invalid promotion: no pawn at target.");
        }

        var promotedPiece = new Piece(move.Promotion.Value, piece.Color);
        state.Board.SetPiece(move.To, promotedPiece);
    }

    private void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == PieceColor.White
            ? PieceColor.Black 
            : PieceColor.White;
    }
}
