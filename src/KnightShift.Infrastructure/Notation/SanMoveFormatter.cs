using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Evaluation;

namespace KnightShift.Infrastructure.Notation;

public class SanMoveFormatter : IMoveFormatter
{
    private readonly GameResultEvaluator _evaluator;

    public SanMoveFormatter(GameResultEvaluator evaluator)
    {
        _evaluator = evaluator;
    }

    public string Format(Move move, GameState stateBeforeMove, GameState stateAfterMove)
    {
        var piece = stateBeforeMove.Board.GetPiece(move.Origin)!;

        string notation = "";

        // 1. Piece letter (no letter for pawn)
        if (piece.Type != PieceType.Pawn)
            notation += ParsePieceType(piece.Type);

        // 2. Capture
        var targetPiece = stateBeforeMove.Board.GetPiece(move.Target);
        bool isCapture = targetPiece != null;

        if (piece.Type == PieceType.Pawn && isCapture)
            notation += move.Origin.File;

        if (isCapture)
            notation += "x";

        // 3. Target square
        notation += move.Target.ToString();

        // 4. Promotion
        if (move.Promotion is not null)
            notation += "=" + ParsePieceType(move.Promotion.Value);

        // 5. Check / Checkmate
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
}
