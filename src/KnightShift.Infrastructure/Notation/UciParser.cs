using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Notation;

public static class UciParser
{
    public static Move FromUci(string uci)
    {
        if (string.IsNullOrWhiteSpace(uci) || (uci.Length != 4 && uci.Length != 5))
            throw new ArgumentException($"Invalid UCI move: {uci}");

        var origin = Position.CreateFromAlgebraic(uci[0..2]);
        var target = Position.CreateFromAlgebraic(uci[2..4]);

        PieceType? promotion = null;

        if (uci.Length == 5)
        {
            promotion = ParsePromotion(uci[4]);
        }

        return new Move(origin, target, Promotion: promotion);
    }

    public static string ToUci(Move move)
    {
        var uci = $"{move.Origin}{move.Target}";

        if (move.Promotion is not null)
        {
            uci += PromotionToChar(move.Promotion.Value);
        }

        return uci;
    }

    private static PieceType ParsePromotion(char symbol)
    {
        return char.ToLower(symbol) switch
        {
            'q' => PieceType.Queen,
            'r' => PieceType.Rook,
            'b' => PieceType.Bishop,
            'n' => PieceType.Knight,
            _ => throw new ArgumentException($"Invalid promotion piece: {symbol}")
        };
    }

    private static char PromotionToChar(PieceType type)
    {
        return type switch
        {
            PieceType.Queen  => 'q',
            PieceType.Rook   => 'r',
            PieceType.Bishop => 'b',
            PieceType.Knight => 'n',
            _ => throw new ArgumentException($"Invalid promotion type: {type}")
        };
    }
}
