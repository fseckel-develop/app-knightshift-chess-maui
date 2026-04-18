using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Serialization;

public class UciMoveSerializer : IMoveSerializer
{
    public Move Deserialize(string uci)
    { 
        if (!IsValidFormat(uci))
            throw new ArgumentException($"Invalid UCI format: {uci}");

        var origin = Position.CreateFromAlgebraic(uci[0..2]);
        var target = Position.CreateFromAlgebraic(uci[2..4]);

        PieceType? promotion = null;

        if (uci.Length == 5)
        {
            promotion = ParsePromotion(uci[4]);
        }

        return new Move(origin, target, Promotion: promotion);
    }

    public string Serialize(Move move)
    {
        var uci = $"{move.Origin}{move.Target}";

        if (move.Promotion is not null)
        {
            uci += PromotionToChar(move.Promotion.Value);
        }

        return uci;
    }

    private static bool IsValidFormat(string uci)
    {
        if (uci.Length is not (4 or 5))
            return false;

        return
            uci[0] is >= 'a' and <= 'h' &&
            uci[1] is >= '1' and <= '8' &&
            uci[2] is >= 'a' and <= 'h' &&
            uci[3] is >= '1' and <= '8' &&
            (uci.Length == 4 || char.ToLower(uci[4]) is 'q' or 'r' or 'b' or 'n');
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
