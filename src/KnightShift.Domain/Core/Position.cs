using KnightShift.Domain.Constants;
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
        if (row < 0 || row >= BoardDimensions.Size || column < 0 || column >= BoardDimensions.Size)
            throw new InvalidPositionException($"Invalid coordinates: ({row}, {column})");

        var file = (char)(BoardDimensions.MinFile + column);
        var rank = BoardDimensions.MaxRank - row;

        return new Position(file, rank);
    }

    public static bool TryCreateFromCoords(int row, int column, out Position position)
    {
        position = null!;
        try
        {
            position = CreateFromCoords(row, column);
            return true;
        }
        catch (InvalidPositionException)
        {
            return false;
        }
    }

    public static Position CreateFromAlgebraic(string notation)
    {
        if (string.IsNullOrWhiteSpace(notation) || notation.Length != 2)
            throw new InvalidPositionException($"Invalid notation: {notation}");

        char file = notation[0];
        int rank = notation[1] - '0';
        var position = new Position(file, rank);

        if (!position.IsValid())
            throw new InvalidPositionException($"Invalid position: {position}");

        return position;
    }

    public static bool TryCreateFromAlgebraic(string notation, out Position position)
    {
        position = null!;
        try
        {
            position = CreateFromAlgebraic(notation);
            return true;
        }
        catch (InvalidPositionException)
        {
            return false;
        }
    }

    public bool IsValid() => 
        File >= BoardDimensions.MinFile && 
        File <= BoardDimensions.MaxFile && 
        Rank >= BoardDimensions.MinRank && 
        Rank <= BoardDimensions.MaxRank;

    public int ToRow() => BoardDimensions.MaxRank - Rank;

    public int ToColumn() => File - BoardDimensions.MinFile;

    public override string ToString() => $"{File}{Rank}";
}
