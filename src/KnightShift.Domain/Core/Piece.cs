using KnightShift.Domain.Enums;

namespace KnightShift.Domain.Core;

public sealed record Piece 
(
    PieceType Type, 
    PieceColor Color
);
