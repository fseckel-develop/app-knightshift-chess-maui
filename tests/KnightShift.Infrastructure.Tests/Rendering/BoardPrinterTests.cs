using KnightShift.Infrastructure.Rendering;
using KnightShift.Infrastructure.Serialization;

namespace KnightShift.Infrastructure.Tests.Rendering;

public class BoardPrinterTests
{
    private readonly FenGameStateSerializer _serializer = new();

    [Fact]
    public void Print_Should_Render_EmptyBoard_With_Grid()
    {
        var state = _serializer.Deserialize("8/8/8/8/8/8/8/8 w - -");
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
        var state = _serializer.Deserialize("8/8/8/8/8/8/8/8 w - -");
        var writer = new StringWriter();

        BoardPrinter.Print(state, writer);
        var output = writer.ToString();

        Assert.Contains(" a ", output);
        Assert.Contains(" h ", output);
    }

    [Fact]
    public void Print_Should_Render_Rank_Labels()
    {
        var state = _serializer.Deserialize("8/8/8/8/8/8/8/8 w - -");
        var writer = new StringWriter();

        BoardPrinter.Print(state, writer);
        var output = writer.ToString();

        Assert.Contains("8 │", output);
        Assert.Contains("1 │", output);
    }

    [Fact]
    public void Print_Should_Render_Pieces()
    {
        var state = _serializer.Deserialize("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");
        var writer = new StringWriter();

        BoardPrinter.Print(state, writer);
        var output = writer.ToString();

        Assert.Contains("♔", output); // white king
        Assert.Contains("♚", output); // black king
        Assert.Contains("♙", output); // pawn
        Assert.Contains("♞", output); // knight
    }
}
