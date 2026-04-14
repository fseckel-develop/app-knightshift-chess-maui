namespace KnightShift.Domain.Constants;

public static class Offsets
{
    public static readonly (int dRow, int dColumn)[] Knight =
    [
        ( 1,  2), ( 2,  1), ( 2, -1), ( 1, -2),
        (-1, -2), (-2, -1), (-2,  1), (-1,  2)
    ];

    public static readonly (int dRow, int dColumn)[] King =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1)
    ];
}

public static class Directions
{
    public static readonly (int dRow, int dColumn)[] Bishop =
    [
        (-1, -1), (-1, 1), (1, -1), (1, 1)
    ];

    public static readonly (int dRow, int dColumn)[] Rook =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1)
    ];

    public static readonly (int dRow, int dColumn)[] Queen =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1)
    ];
}
