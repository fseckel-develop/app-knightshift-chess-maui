using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Core;

public sealed record Move
(
    Position Origin,
    Position Target,
    PieceType? Promotion = null,
    bool IsCastling = false,
    bool IsEnPassant = false
);
