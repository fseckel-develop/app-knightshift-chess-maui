using KnightShift.Cli.Rendering;
using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Application.Contracts.DTOs;
using NSubstitute;

namespace KnightShift.Cli.Tests.Rendering;

public class UiRendererSequentialTests
{
    private readonly IContentResolver _resolver = Substitute.For<IContentResolver>([]);
    private readonly UiRenderer _renderer;

    public UiRendererSequentialTests()
    {
        _renderer = new UiRenderer(_resolver);
    }

    private static UiState CreateState()
    {
        return new UiState
        {
            Mode = UiMode.Sequential,
            Game = new GameStateDto(),
            StatusMessage = "hello",
            ContentType = UiContent.History
        };
    }

    [Fact]
    public void Render_Should_Show_Message()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns([]);

        var state = CreateState();

        var result = _renderer.Render(state);

        Assert.Contains("hello", result);
    }

    [Fact]
    public void Render_Should_Show_Content()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns(["line1", "line2"]);

        var state = CreateState();

        var result = _renderer.Render(state);

        Assert.Contains("line1", result);
        Assert.Contains("line2", result);
    }

    [Fact]
    public void Render_Should_Print_Board_When_Enabled()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns([]);

        var state = CreateState();
        state.PrintBoard = true;

        var result = _renderer.Render(state);

        Assert.Contains("┌", result);
        Assert.Contains("a", result);
    }

    [Fact]
    public void Render_Should_Not_Print_Board_When_Disabled()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns([]);

        var state = CreateState();
        state.PrintBoard = false;

        var result = _renderer.Render(state);

        Assert.DoesNotContain("┌", result);
        Assert.DoesNotContain("a", result);
    }

    [Fact]
    public void Render_Should_Add_Empty_Line_After_Output()
    {
        _resolver.Resolve(Arg.Any<UiState>())
            .Returns([]);

        var state = CreateState();

        var result = _renderer.Render(state);

        Assert.EndsWith(Environment.NewLine, result);
    }
}
