using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Domain.Constants;

namespace KnightShift.Infrastructure.Rendering;

public static class BoardPrinter
{
    public static void Print(GameState state)
    {
        var board = state.Board;
        var lastMove = state.MoveHistory.LastOrDefault();

        Console.WriteLine();
        PrintTopBorder();

        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            int rank = BoardDimensions.MaxRank - row;
            Console.Write($"{rank} │");

            for (int column = 0; column < BoardDimensions.Size; column++)
            {
                var position = Position.CreateFromCoords(row, column);
                var piece = board.GetPiece(position);

                bool isDarkSquare = IsDarkSquare(row, column);
                bool isHighlighted = IsHighlighted(position, lastMove);

                PrintSquare(piece, isDarkSquare, isHighlighted);
            }

            Console.WriteLine("│");

            if (row < BoardDimensions.Size - 1)
                PrintMiddleBorder();
        }

        PrintBottomBorder();
        PrintFiles();
    }

    private static bool IsDarkSquare(int row, int col)
    {
        return (row + col) % 2 == 1;
    }

    private static bool IsHighlighted(Position position, Move? lastMove)
    {
        return lastMove is not null 
            && (position == lastMove.Origin || position == lastMove.Target);
    }

    private static void PrintSquare(Piece? piece, bool isDarkSquare, bool highlight)
    {
        SetBackgroundColor(isDarkSquare, highlight);

        Console.Write(" ");

        if (piece is null)
        {
            Console.Write(".");
        }
        else
        {
            SetPieceColor(piece);
            Console.Write(GetUnicodeSymbol(piece));
        }

        Console.Write(" ");

        Console.ResetColor();
        Console.Write("│");
    }

    private static void SetBackgroundColor(bool isDarkSquare, bool highlight)
    {
        if (highlight)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
        }
        else
        {
            Console.BackgroundColor = isDarkSquare
                ? ConsoleColor.DarkGreen
                : ConsoleColor.Black;
        }
    }

    private static void SetPieceColor(Piece piece)
    {
        Console.ForegroundColor = piece.Color == PieceColor.White
            ? ConsoleColor.White
            : ConsoleColor.DarkGray;
    }

    private static string GetUnicodeSymbol(Piece piece)
    {
        return piece.Type switch
        {
            PieceType.King   => piece.Color == PieceColor.White ? "♔" : "♚",
            PieceType.Queen  => piece.Color == PieceColor.White ? "♕" : "♛",
            PieceType.Rook   => piece.Color == PieceColor.White ? "♖" : "♜",
            PieceType.Bishop => piece.Color == PieceColor.White ? "♗" : "♝",
            PieceType.Knight => piece.Color == PieceColor.White ? "♘" : "♞",
            PieceType.Pawn   => piece.Color == PieceColor.White ? "♙" : "♟",
            _ => "?"
        };
    }

    private static void PrintTopBorder()
    {
        Console.WriteLine("  ┌───┬───┬───┬───┬───┬───┬───┬───┐");
    }

    private static void PrintMiddleBorder()
    {
        Console.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
    }

    private static void PrintBottomBorder()
    {
        Console.WriteLine("  └───┴───┴───┴───┴───┴───┴───┴───┘");
    }

    private static void PrintFiles()
    {
        Console.Write("    ");
        for (char file = BoardDimensions.MinFile; file <= BoardDimensions.MaxFile; file++)
        {
            Console.Write($" {file} ");
        }
        Console.WriteLine();
    }
}
