using KnightShift.Application.Contracts.DTOs;
using KnightShift.Domain.Constants;
using KnightShift.Domain.Enums;

namespace KnightShift.Cli.Rendering;

public static class BoardPrinter
{
    public static void Print(GameStateDto state)
    {
        using var writer = new StringWriter();

        PrintBoard(state, writer);

        FramedToConsole(writer.ToString());
    }

    public static void PrintBoard(GameStateDto state, TextWriter writer)
    {
        var board = state.Board;
        var lastMove = state.LastMove;

        PrintFiles(writer);
        PrintTopBorder(writer);

        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            int rank = BoardDimensions.MaxRank - row;

            SetForeground(writer, 180, 180, 180);
            writer.Write($" {rank} ");
            ResetColors(writer);

            SetForeground(writer, 80, 80, 80);
            writer.Write("│");
            ResetColors(writer);

            for (int column = 0; column < BoardDimensions.Size; column++)
            {
                var piece = board[row, column];

                bool isDarkSquare = IsDarkSquare(row, column);
                bool isHighlighted = IsHighlighted(row, column, lastMove);

                PrintSquare(writer, piece, isDarkSquare, isHighlighted);
            }

            SetForeground(writer, 180, 180, 180);
            writer.Write($" {rank} ");
            ResetColors(writer);

            writer.WriteLine("");

            if (row < 7)
                PrintMiddleBorder(writer);
        }

        PrintBottomBorder(writer);
        PrintFiles(writer);
    }

    private static bool IsDarkSquare(int row, int column)
    {
        return (row + column) % 2 == 1;
    }

    private static bool IsHighlighted(int row, int column, MoveDto? lastMove)
    {
        if (lastMove is null)
            return false;

        return
            lastMove.OriginRow == row && lastMove.OriginColumn == column ||
            lastMove.TargetRow == row && lastMove.TargetColumn == column;
    }

    private static void PrintSquare(TextWriter writer, PieceDto? piece, bool isDark, bool isHighlighted)
    {
        if (isHighlighted)
            SetBackground(writer, 246, 246, 105);
        else if (isDark)
            SetBackground(writer, 181, 136, 99);
        else
            SetBackground(writer, 240, 217, 181);

        if (piece is null)
        {
            writer.Write("   ");
        }
        else
        {
            if (piece.Color == PieceColor.White)
                SetForeground(writer, 255, 255, 255);
            else
                SetForeground(writer, 30, 30, 30);

            writer.Write($" {GetUnicodeSymbol(piece)} ");
        }

        ResetColors(writer);

        SetForeground(writer, 80, 80, 80);
        writer.Write("│");
        ResetColors(writer);
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
        SetForeground(writer, 80, 80, 80);
        writer.WriteLine("   ┌───┬───┬───┬───┬───┬───┬───┬───┐");
        ResetColors(writer);
    }

    private static void PrintMiddleBorder(TextWriter writer)
    {
        SetForeground(writer, 80, 80, 80);
        writer.WriteLine("   ├───┼───┼───┼───┼───┼───┼───┼───┤");
        ResetColors(writer);
    }

    private static void PrintBottomBorder(TextWriter writer)
    {
        SetForeground(writer, 80, 80, 80);
        writer.WriteLine("   └───┴───┴───┴───┴───┴───┴───┴───┘");
        ResetColors(writer);
    }

    private static void PrintFiles(TextWriter writer)
    {
        writer.Write("    ");

        for (char file = 'a'; file <= 'h'; file++)
        {
            SetForeground(writer, 180, 180, 180);
            writer.Write($" {file}  ");
            ResetColors(writer);
        }

        writer.WriteLine();
    }

    private static void SetBackground(TextWriter writer, int r, int g, int b)
    {
        writer.Write($"\x1b[48;2;{r};{g};{b}m");
    }

    private static void SetForeground(TextWriter writer, int r, int g, int b)
    {
        writer.Write($"\x1b[38;2;{r};{g};{b}m");
    }

    private static void ResetColors(TextWriter writer)
    {
        writer.Write("\x1b[0m");
    }

    private static void FramedToConsole(string content)
    {
        var lines = content.Split(Environment.NewLine);

        // remove last empty line if present
        if (lines.Length > 0 && string.IsNullOrWhiteSpace(lines[^1]))
            lines = lines[..^1];

        int width = lines.Max(GetVisibleLength);

        // top border
        SetForeground(Console.Out, 120, 120, 120);
        Console.WriteLine($"┌{new string('─', width)}┐");

        foreach (var line in lines)
        {
            SetForeground(Console.Out, 120,120,120);
            Console.Write("│");
            ResetColors(Console.Out);
            Console.Write(line);

            int padding = width - GetVisibleLength(line);
            Console.Write(new string(' ', padding));

            SetForeground(Console.Out, 120,120,120);
            Console.WriteLine("│");
            ResetColors(Console.Out);
        }

        SetForeground(Console.Out, 120, 120, 120);
        Console.WriteLine($"└{new string('─', width)}┘");
        ResetColors(Console.Out);
    }

    private static int GetVisibleLength(string input)
    {
        int length = 0;
        bool inEscape = false;

        foreach (char symbol in input)
        {
            if (symbol == '\x1b')
                inEscape = true;
            else if (inEscape && symbol == 'm')
                inEscape = false;
            else if (!inEscape)
                length++;
        }

        return length;
    }
}
