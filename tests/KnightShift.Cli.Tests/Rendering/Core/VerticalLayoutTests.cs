using KnightShift.Cli.Rendering.Core;

namespace KnightShift.Cli.Tests.Rendering.Core;

public class VerticalLayoutTests
{
    [Fact]
    public void Combine_Should_Stack_Blocks()
    {
        var result = VerticalLayout.Combine("A", "B");

        Assert.Contains("A", result);
        Assert.Contains("B", result);
    }

    [Fact]
    public void Combine_Should_Add_NewLines_Between_Blocks()
    {
        var result = VerticalLayout.Combine("A", "B");
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(2, lines.Length);
    }
}
