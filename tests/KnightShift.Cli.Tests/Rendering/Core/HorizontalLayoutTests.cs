using KnightShift.Cli.Rendering.Core;

namespace KnightShift.Cli.Tests.Rendering.Core;

public class HorizontalLayoutTests
{
    [Fact]
    public void Combine_Should_Align_Lines_With_Spacing()
    {
        var left = "A\nB";
        var right = "1\n2";

        var result = HorizontalLayout.Combine(left, right, spacing: 1);
        var lines = result.Split('\n');

        Assert.Equal("A 1", lines[0]);
        Assert.Equal("B 2", lines[1]);
    }

    [Fact]
    public void Combine_Should_Handle_Different_Heights()
    {
        var left = "A\nB\nC";
        var right = "1";

        var result = HorizontalLayout.Combine(left, right, spacing: 1);
        var lines = result.Split('\n');

        Assert.Equal("A 1", lines[0]);
        Assert.Equal("B ", lines[1]);
        Assert.Equal("C ", lines[2]);
    }

    [Fact]
    public void Combine_Should_Return_Left_When_Right_Is_Empty()
    {
        var left = "A\nB";

        var result = HorizontalLayout.Combine(left, "");

        Assert.Equal(left, result);
    }
}
