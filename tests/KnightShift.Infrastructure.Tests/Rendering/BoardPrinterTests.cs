using KnightShift.Infrastructure.Rendering;
using KnightShift.Infrastructure.Notation;
using KnightShift.Domain.Core;

namespace KnightShift.Infrastructure.Tests.Rendering;

public class BoardPrinterTests
{
    [Fact]
    public void Print_Should_Render_EmptyBoard_With_Grid()
    {
        var state = FenParser.FromFen("8/8/8/8/8/8/8/8 w - -");
        var writer = new StringWriter();

        BoardPrinter.Print(state, writer);
        var output = writer.ToString();

        Assert.Contains("┌", output);
        Assert.Contains("└", output);
        Assert.Contains("├", output);
        Assert.Contains("│", output);
    }

    [Fact]
    public void Print_Should_Render_File_Labels()
    {
        var state = FenParser.FromFen("8/8/8/8/8/8/8/8 w - -");
        var writer = new StringWriter();

        BoardPrinter.Print(state, writer);
        var output = writer.ToString();

        Assert.Contains(" a ", output);
        Assert.Contains(" h ", output);
    }

    [Fact]
    public void Print_Should_Render_Rank_Labels()
    {
        var state = FenParser.FromFen("8/8/8/8/8/8/8/8 w - -");
        var writer = new StringWriter();

        BoardPrinter.Print(state, writer);
        var output = writer.ToString();

        Assert.Contains("8 │", output);
        Assert.Contains("1 │", output);
    }

    [Fact]
    public void Print_Should_Render_Pieces()
    {
        var state = FenParser.FromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");
        var writer = new StringWriter();

        BoardPrinter.Print(state, writer);
        var output = writer.ToString();

        Assert.Contains("♔", output); // white king
        Assert.Contains("♚", output); // black king
        Assert.Contains("♙", output); // pawn
        Assert.Contains("♞", output); // knight
    }
}
