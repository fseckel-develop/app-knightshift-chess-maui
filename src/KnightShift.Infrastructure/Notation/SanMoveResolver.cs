using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves;

namespace KnightShift.Infrastructure.Notation;

public class SanMoveResolver
{
    private sealed record SanConstraints(
        PieceType Piece,
        char? OriginFile,
        int? OriginRank,
        string Target,
        bool IsCapture,
        PieceType? Promotion,
        bool IsCastlingKingSide,
        bool IsCastlingQueenSide
    );

    private readonly IMoveGenerator _moveGenerator;

    public SanMoveResolver(IMoveGenerator moveGenerator)
    {
        _moveGenerator = moveGenerator;
    }

    public Move Resolve(string san, GameState state)
    {
        var constraints = ParseSan(san);
        var legalMoves = _moveGenerator.GenerateMoves(state);

        if (constraints.IsCastlingKingSide || constraints.IsCastlingQueenSide)
        {
            return legalMoves.First(move =>
                move.IsCastling &&
                (constraints.IsCastlingKingSide
                    ? move.Target.File > move.Origin.File
                    : move.Target.File < move.Origin.File));
        }

        var target = Position.CreateFromAlgebraic(constraints.Target);

        var candidates = legalMoves.Where(move =>
        {
            if (state.Board.GetPiece(move.Origin)!.Type != constraints.Piece)
                return false;
    
            if (constraints.OriginFile is not null && move.Origin.File != constraints.OriginFile)
                return false;

            if (constraints.OriginRank is not null && move.Origin.Rank != constraints.OriginRank)
                return false;

            if (move.Target != target)
                return false;

            if (constraints.Promotion is not null && move.Promotion != constraints.Promotion)
                return false;

            return true;
        })
        .ToList();

        if (candidates.Count == 1)
            return candidates[0];

        if (candidates.Count == 0)
            throw new InvalidOperationException($"Invalid move: {san}");

        throw new InvalidOperationException($"Ambiguous move: {san}");
    }

    private static SanConstraints ParseSan(string san)
    {
        san = san.Trim();
        san = san.Replace("+", "").Replace("#", "");

        if (san == "O-O")
            return new(PieceType.King, null, null, "", false, null, true, false);

        if (san == "O-O-O")
            return new(PieceType.King, null, null, "", false, null, false, true);

        PieceType? promotion = null;
        var promotionIndex = san.IndexOf('=');
        if (promotionIndex >= 0)
        {
            promotion = ParsePieceType(san[promotionIndex + 1]);
            san = san[..promotionIndex];
        }

        var target = san[^2..];

        var prefix = san[..^2];

        var piece = ParsePieceType(prefix.FirstOrDefault());
        if (piece != PieceType.Pawn)
            prefix = prefix[1..];

        bool isCapture = false;
        char? originFile = null;
        int? originRank = null;
        foreach (var symbol in prefix)
        {
            if (symbol == 'x')
            {
                isCapture = true;
                continue;
            }

            if (symbol is >= 'a' and <= 'h')
                originFile = symbol;

            else if (symbol is >= '1' and <= '8')
                originRank = symbol - '0';
        }

        return new(piece, originFile, originRank, target, isCapture, promotion, false, false);
    }

    private static PieceType ParsePieceType(char symbol) => symbol switch
    {
        'K' => PieceType.King,
        'Q' => PieceType.Queen,
        'R' => PieceType.Rook,
        'B' => PieceType.Bishop,
        'N' => PieceType.Knight,
        _ => PieceType.Pawn
    };
}
