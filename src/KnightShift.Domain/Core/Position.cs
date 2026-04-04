using KnightShift.Domain.Exceptions;

namespace KnightShift.Domain.Core;

public sealed record Position
(
    char File, 
    int Rank
)
{
    public static Position CreateFromCoords(int row, int column)
    {
        if (row < 0 || row >= 8 || column < 0 || column >= 8)
            throw new InvalidPositionException($"Invalid coordinates: ({row}, {column})");

        var file = (char)('a' + column);
        var rank = 8 - row;

        return new Position(file, rank);
    }

    public bool IsValid() => File >= 'a' && File <= 'h' && Rank >= 1 && Rank <= 8;

    public int ToRow() => 8 - Rank;

    public int ToColumn() => File - 'a';

    public override string ToString() => $"{File}{Rank}";
}
