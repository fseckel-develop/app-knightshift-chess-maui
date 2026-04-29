using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Domain.Core;
using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Tests.Helpers;
using NSubstitute;

namespace KnightShift.Cli.Tests.Rendering.Content;

public class HistoryContentProviderTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly IMoveFormatter _formatter = Substitute.For<IMoveFormatter>();

    private HistoryContentProvider Create()
        => new(_game, _formatter);
    
    [Fact]
    public void GetContent_Should_Return_Empty_When_No_History()
    {
        _game.GetHistory().Returns([]);

        var provider = Create();

        var result = provider.GetContent(new UiState { Mode = UiMode.Dashboard });

        Assert.Equal([""], result);
    }

    [Fact]
    public void GetDashboardHistory_Should_Format_Two_Columns()
    {
        var history = TestData.History(2).ToList();

        _game.GetHistory().Returns(history);

        _formatter.Format(Arg.Any<Move>(), Arg.Any<GameState>(), Arg.Any<GameState>())
            .Returns("e4", "e5");

        var provider = Create();

        var result = provider.GetContent(new UiState { Mode = UiMode.Dashboard });

        Assert.Contains("1.", result[0]);
        Assert.Contains("e4", result[0]);
        Assert.Contains("e5", result[0]);
    }

    [Fact]
    public void GetSequentialHistory_Should_Inline_Moves()
    {
        var history = TestData.History(4).ToList();

        _game.GetHistory().Returns(history);

        _formatter.Format(Arg.Any<Move>(), Arg.Any<GameState>(), Arg.Any<GameState>())
            .Returns("e4");

        var provider = Create();

        var result = provider.GetContent(new UiState { Mode = UiMode.Sequential });

        var joined = string.Join(" ", result);

        Assert.Contains("1.", joined);
        Assert.Contains("2.", joined);
    }

    [Fact]
    public void GetSequentialHistory_Should_Wrap_Long_Lines()
    {
        var history = TestData.History(20).ToList();

        _game.GetHistory().Returns(history);

        _formatter.Format(Arg.Any<Move>(), Arg.Any<GameState>(), Arg.Any<GameState>())
            .Returns("longmove");

        var provider = Create();

        var result = provider.GetContent(new UiState { Mode = UiMode.Sequential });

        Assert.True(result.Length > 1); // wrapping happened
    }
}
