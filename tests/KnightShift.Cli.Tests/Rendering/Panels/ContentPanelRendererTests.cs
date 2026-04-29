using KnightShift.Cli.Rendering.Panels;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Tests.Rendering.Panels;

public class ContentPanelRendererTests
{
    [Fact]
    public void Render_Should_Include_Title()
    {
        var result = ContentPanelRenderer.Render(UiContent.Moves, ["A"], 5);

        Assert.Contains("Legal Moves", result);
    }

    [Fact]
    public void Render_Should_Return_Frame()
    {
        var result = ContentPanelRenderer.Render(UiContent.History, ["A"], 5);

        Assert.Contains("┌", result);
        Assert.Contains("└", result);
    }

    [Fact]
    public void Render_Should_Respect_Height()
    {
        var content = new[] { "A", "B", "C" };

        var result = ContentPanelRenderer.Render(UiContent.Moves, content, 4);
        var lines = result.Split('\n');

        Assert.True(lines.Length >= 4);
    }

    [Fact]
    public void Render_Should_Trim_Long_Lines()
    {
        var content = new[] { new string('x', 100) };

        var result = ContentPanelRenderer.Render(UiContent.Moves, content, 5);

        Assert.DoesNotContain(new string('x', 100), result);
    }
}
