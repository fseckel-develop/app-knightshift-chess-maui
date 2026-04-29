using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Tests.Helpers;
using NSubstitute;

namespace KnightShift.Cli.Tests.Rendering.Content;

public class MovesContentProviderTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();

    private MovesContentProvider Create()
        => new(_game);

    [Fact]
    public void GetContent_Should_Return_Empty_When_No_Moves()
    {
        _game.GetLegalMoves().Returns([]);

        var provider = Create();

        var result = provider.GetContent(new UiState { Mode = UiMode.Sequential });

        Assert.Equal([""], result);
    }

    [Fact]
    public void GetDashboardMoves_Should_Format_Three_Columns()
    {
        _game.GetLegalMoves().Returns(
            TestData.ManyMoveDtos(
                ("a2", "a3"),
                ("a2", "a4"),
                ("b2", "b3")
            )
        );

        var provider = Create();

        var result = provider.GetContent(new UiState { Mode = UiMode.Dashboard });

        Assert.Single(result);
        Assert.Contains("a2a3", result[0]);
        Assert.Contains("a2a4", result[0]);
        Assert.Contains("b2b3", result[0]);
    }

    [Fact]
    public void GetSequentialMoves_Should_Group_By_8()
    {
        var moves = Enumerable.Range(0, 10)
            .Select(i => ("a2", $"a{i}"))
            .ToArray();

        _game.GetLegalMoves().Returns(TestData.ManyMoveDtos(moves));

        var provider = Create();

        var result = provider.GetContent(new UiState { Mode = UiMode.Sequential });

        Assert.True(result.Length >= 2);
    }

    [Fact]
    public void GetMoves_Should_Filter_By_Square()
    {
        _game.GetLegalMoves("e2").Returns(
            TestData.ManyMoveDtos(("e2", "e4"))
        );

        var provider = Create();

        var state = new UiState
        {
            Mode = UiMode.Sequential,
            ContentState = new MovesContentState { OriginSquare = "e2" }
        };

        var result = provider.GetContent(state);

        Assert.Contains("e2e4", string.Join(" ", result));
    }
}
