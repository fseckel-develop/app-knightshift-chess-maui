using KnightShift.Application.Contracts.DTOs;
using KnightShift.Domain.Constants;
using KnightShift.Domain.Enums;

namespace KnightShift.Cli.Rendering;

public static class BoardPrinter
{
    public static void Print(GameStateDto state)
    {
        Print(state, Console.Out);
    }

    public static void Print(GameStateDto state, TextWriter writer)
    {
        var board = state.Board;
        var lastMove = state.LastMove;

        writer.WriteLine();
        PrintTopBorder(writer);

        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            int rank = BoardDimensions.MaxRank - row;
            writer.Write($"{rank} │");

            for (int column = 0; column < BoardDimensions.Size; column++)
            {
                var piece = board[row, column];

                bool isDarkSquare = IsDarkSquare(row, column);
                bool isHighlighted = IsHighlighted(row, column,lastMove);

                PrintSquare(writer, piece, isDarkSquare, isHighlighted);
            }

            writer.WriteLine();

            if (row < BoardDimensions.Size - 1)
                PrintMiddleBorder(writer);
        }

        PrintBottomBorder(writer);
        PrintFiles(writer);
    }

    private static bool IsDarkSquare(int row, int col)
    {
        return (row + col) % 2 == 1;
    }

    private static bool IsHighlighted(int row, int column, MoveDto? lastMove)
    {
        if (lastMove is null)
            return false;

        return
            lastMove.OriginRow == row && lastMove.OriginColumn == column ||
            lastMove.TargetRow == row && lastMove.TargetColumn == column;
    }

    private static void PrintSquare(TextWriter writer, PieceDto? piece, bool isDarkSquare, bool highlight)
    {
        if (UseConsoleColors(writer))
            SetBackgroundColor(isDarkSquare, highlight);

        writer.Write(" ");

        if (piece is null)
        {
            writer.Write(" ");
        }
        else
        {
            if (UseConsoleColors(writer))
                SetPieceColor(piece);
            
            writer.Write(GetUnicodeSymbol(piece));
        }

        writer.Write(" ");

        if (UseConsoleColors(writer))
            Console.ResetColor();

        writer.Write("│");
    }

    private static bool UseConsoleColors(TextWriter writer)
        => ReferenceEquals(writer, Console.Out);

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

    private static void SetPieceColor(PieceDto piece)
    {
        Console.ForegroundColor = piece.Color == PieceColor.White
            ? ConsoleColor.White
            : ConsoleColor.DarkGray;
    }

    private static string GetUnicodeSymbol(PieceDto piece)
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

    private static void PrintTopBorder(TextWriter writer)
    {
        writer.WriteLine("  ┌───┬───┬───┬───┬───┬───┬───┬───┐");
    }

    private static void PrintMiddleBorder(TextWriter writer)
    {
        writer.WriteLine("  ├───┼───┼───┼───┼───┼───┼───┼───┤");
    }

    private static void PrintBottomBorder(TextWriter writer)
    {
        writer.WriteLine("  └───┴───┴───┴───┴───┴───┴───┴───┘");
    }

    private static void PrintFiles(TextWriter writer)
    {
        writer.Write("   ");
        for (char file = BoardDimensions.MinFile; file <= BoardDimensions.MaxFile; file++)
        {
            writer.Write($" {file}  ");
        }
        writer.WriteLine();
    }
}
