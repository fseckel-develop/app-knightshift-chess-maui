using KnightShift.Application.Contracts.DTOs;
using KnightShift.Domain.Constants;
using KnightShift.Cli.Rendering.Core;

namespace KnightShift.Cli.Rendering.Panels;

public static class BoardPanelRenderer
{
    public static string Render(GameStateDto state)
    {
        using var renderer = new StringWriter();

        var board = state.Board;
        var lastMove = state.LastMove;

        RenderFiles(renderer);
        RenderTopBorder(renderer);

        for (int row = 0; row < BoardDimensions.Size; row++)
        {
            int rank = BoardDimensions.MaxRank - row;

            renderer.Write(Ansi.Foreground(180, 180, 180));
            renderer.Write($" {rank} ");
            renderer.Write(Ansi.ResetColor());

            renderer.Write(Ansi.Foreground(80, 80, 80));
            renderer.Write("│");
            renderer.Write(Ansi.ResetColor());

            for (int column = 0; column < BoardDimensions.Size; column++)
            {
                var piece = board[row, column];

                bool isDarkSquare = IsDarkSquare(row, column);
                bool isHighlighted = IsHighlighted(row, column, lastMove);

                PrintSquare(renderer, piece, isDarkSquare, isHighlighted);
            }

            renderer.Write(Ansi.Foreground(180, 180, 180));
            renderer.Write($" {rank} ");
            renderer.Write(Ansi.ResetColor());

            renderer.WriteLine(Ansi.ResetColor());

            if (row < 7)
                RenderMiddleBorder(renderer);
        }

        RenderBottomBorder(renderer);
        RenderFiles(renderer);

        return FrameRenderer.RenderFrame(renderer.ToString());
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
            writer.Write(Ansi.Background(246, 246, 105));
        else if (isDark)
            writer.Write(Ansi.Background(181, 136, 99));
        else
            writer.Write(Ansi.Background(240, 217, 181));

        if (piece is null)
        {
            writer.Write("   ");
        }
        else
        {
            if (piece.Color == PieceColorDto.White)
                writer.Write(Ansi.Foreground(255, 255, 255));
            else
                writer.Write(Ansi.Foreground(30, 30, 30));

            writer.Write($" {GetUnicodeSymbol(piece)} ");
        }

        writer.Write(Ansi.ResetColor());

        writer.Write(Ansi.Foreground(80, 80, 80));
        writer.Write("│");
        writer.Write(Ansi.ResetColor());
    }

    private static string GetUnicodeSymbol(PieceDto piece)
    {
        return piece.Type switch
        {
            PieceTypeDto.King   => piece.Color == PieceColorDto.White ? "♔" : "♚",
            PieceTypeDto.Queen  => piece.Color == PieceColorDto.White ? "♕" : "♛",
            PieceTypeDto.Rook   => piece.Color == PieceColorDto.White ? "♖" : "♜",
            PieceTypeDto.Bishop => piece.Color == PieceColorDto.White ? "♗" : "♝",
            PieceTypeDto.Knight => piece.Color == PieceColorDto.White ? "♘" : "♞",
            PieceTypeDto.Pawn   => piece.Color == PieceColorDto.White ? "♙" : "♟",
            _ => "?"
        };
    }

    private static void RenderTopBorder(TextWriter writer)
    {
        writer.Write(Ansi.Foreground(80, 80, 80));
        writer.WriteLine("   ┌───┬───┬───┬───┬───┬───┬───┬───┐");
        writer.Write(Ansi.ResetColor());
    }

    private static void RenderMiddleBorder(TextWriter writer)
    {
        writer.Write(Ansi.Foreground(80, 80, 80));
        writer.WriteLine("   ├───┼───┼───┼───┼───┼───┼───┼───┤");
        writer.Write(Ansi.ResetColor());
    }

    private static void RenderBottomBorder(TextWriter writer)
    {
        writer.Write(Ansi.Foreground(80, 80, 80));
        writer.WriteLine("   └───┴───┴───┴───┴───┴───┴───┴───┘");
        writer.Write(Ansi.ResetColor());
    }

    private static void RenderFiles(TextWriter writer)
    {
        writer.Write("    ");

        for (char file = 'a'; file <= 'h'; file++)
        {
            writer.Write(Ansi.Foreground(180, 180, 180));
            writer.Write($" {file}  ");
            writer.Write(Ansi.ResetColor());
        }

        writer.WriteLine();
    }
}
