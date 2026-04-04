using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Core;

public sealed record Move
(
    Position From,
    Position To,
    PieceType? Promotion = null,
    bool IsCastling = false,
    bool IsEnPassant = false
);
