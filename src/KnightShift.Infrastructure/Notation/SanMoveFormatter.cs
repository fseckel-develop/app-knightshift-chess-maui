using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Evaluation;
using KnightShift.Engine.Moves;

namespace KnightShift.Infrastructure.Notation;

public class SanMoveFormatter : IMoveFormatter
{
    private readonly GameResultEvaluator _evaluator;
    private readonly IMoveGenerator _generator;

    public SanMoveFormatter(GameResultEvaluator evaluator, IMoveGenerator generator)
    {
        _evaluator = evaluator;
        _generator = generator;
    }

    public string Format(Move move, GameState stateBeforeMove, GameState stateAfterMove)
    {
        // 1. Castling 
        if (move.IsCastling)
            return move.Target.File > move.Origin.File ? "O-O" : "O-O-O";

        var piece = stateBeforeMove.Board.GetPiece(move.Origin)!;
        string notation = "";

        // 2. Piece (no letter for pawn) + Disambiguation
        if (piece.Type != PieceType.Pawn)
        {
            notation += ParsePieceType(piece.Type);
            notation += GetDisambiguation(move, stateBeforeMove);
        }

        // 3. Capture
        var targetPiece = stateBeforeMove.Board.GetPiece(move.Target);
        bool isCapture = move.IsEnPassant || targetPiece != null;

        if (piece.Type == PieceType.Pawn && isCapture)
            notation += move.Origin.File;

        if (isCapture)
            notation += "x";

        // 4. Target
        notation += move.Target.ToString();

        // 5. Promotion
        if (move.Promotion is not null)
            notation += "=" + ParsePieceType(move.Promotion.Value);

        // 6. Check / Checkmate
        _evaluator.Evaluate(stateAfterMove);

        if (_evaluator.IsKingInCheck(stateAfterMove))
        {
            var isCheckmate = 
                stateAfterMove.Result == GameResult.WhiteWins || 
                stateAfterMove.Result == GameResult.BlackWins;

            notation += isCheckmate ? "#" : "+";
        }

        return notation;
    }

    private static string ParsePieceType(PieceType type) => type switch
    {
        PieceType.King => "K",
        PieceType.Queen => "Q",
        PieceType.Rook => "R",
        PieceType.Bishop => "B",
        PieceType.Knight => "N",
        _ => ""
    };

    private string GetDisambiguation(Move move, GameState state)
    {
        var piece = state.Board.GetPiece(move.Origin)!;

        var candidates = _generator.GenerateMoves(state)
            .Where(candidate =>
                candidate.Target == move.Target &&
                candidate.Origin != move.Origin &&
                state.Board.GetPiece(candidate.Origin)?.Type == piece.Type)
            .ToList();

        if (candidates.Count == 0)
            return "";

        bool sameFile = candidates.Any(candidate => candidate.Origin.File == move.Origin.File);
        if (!sameFile)
            return move.Origin.File.ToString();
        
        bool sameRank = candidates.Any(candidate => candidate.Origin.Rank == move.Origin.Rank);
        if (!sameRank)
            return move.Origin.Rank.ToString();

        return move.Origin.ToString();
    }
}
