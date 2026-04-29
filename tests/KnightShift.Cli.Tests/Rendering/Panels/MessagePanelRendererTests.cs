using KnightShift.Cli.Rendering.Panels;

namespace KnightShift.Cli.Tests.Rendering.Panels;

public class MessagePanelRendererTests
{
    [Fact]
    public void Render_Should_Return_Frame()
    {
        var result = MessagePanelRenderer.Render("hello", 20);

        Assert.Contains("┌", result);
        Assert.Contains("└", result);
    }

    [Fact]
    public void Render_Should_Handle_Empty_Message()
    {
        var result = MessagePanelRenderer.Render("", 20);

        Assert.Contains("┌", result);
    }

    [Fact]
    public void Render_Should_Trim_Long_Message()
    {
        var result = MessagePanelRenderer.Render("this is a very long message", 10);

        Assert.DoesNotContain("this is a very long message", result);
    }
}
