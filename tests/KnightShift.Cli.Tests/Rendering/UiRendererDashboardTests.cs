using KnightShift.Cli.Rendering;
using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Application.Contracts.DTOs;
using NSubstitute;

namespace KnightShift.Cli.Tests.Rendering;

public class UiRendererDashboardTests
{
    private readonly IContentResolver _resolver = Substitute.For<IContentResolver>([]);
    private readonly UiRenderer _renderer;

    public UiRendererDashboardTests()
    {
        _renderer = new UiRenderer(_resolver);
    }

    private static UiState CreateState()
    {
        return new UiState
        {
            Mode = UiMode.Dashboard,
            Game = new GameStateDto(),
            StatusMessage = "hello",
            ContentType = UiContent.History
        };
    }

    [Fact]
    public void Render_Should_Include_Message()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns(["content"]);

        var state = CreateState();

        var result = _renderer.Render(state);

        Assert.Contains("hello", result);
    }

    [Fact]
    public void Render_Should_Include_Content()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns(["right-panel"]);

        var state = CreateState();

        var result = _renderer.Render(state);

        Assert.Contains("right-panel", result);
    }

    [Fact]
    public void Render_Should_Combine_Left_And_Right()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns(["X"]);

        var state = CreateState();

        var result = _renderer.Render(state);

        Assert.Contains("│", result); // frame borders present
    }
}
