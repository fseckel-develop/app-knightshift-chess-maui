using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Tests.Helpers;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class ListCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly ListCommand _command;

    public ListCommandTests()
    {
        _command = new ListCommand(_game);
    }

    [Theory]
    [InlineData("list")]
    [InlineData("moves")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_List_All_Moves()
    {
        _game.GetLegalMoves().Returns(
            TestData.ManyMoveDtos(("a2", "a3"), ("a2", "a4"))
        );

        var result = await _command.ExecuteAsync("list");

        Assert.Equal(UiContent.Moves, result.ContentType);
        Assert.Equal("Found 2 legal moves.", result.Message);
        Assert.NotNull(result.ContentState);
    }

    [Fact]
    public async Task Execute_Should_List_Moves_From_Square()
    {
        _game.GetLegalMoves("e2").Returns(TestData.ManyMoveDtos(("e2", "e4")));

        var result = await _command.ExecuteAsync("list e2");

        Assert.Equal("Found 1 legal move from e2.", result.Message);

        var state = Assert.IsType<MovesContentState>(result.ContentState);
        Assert.Equal("e2", state.OriginSquare);
    }

    [Fact]
    public async Task Execute_Should_Handle_No_Moves()
    {
        _game.GetLegalMoves().Returns([]);

        var result = await _command.ExecuteAsync("list");

        Assert.Equal("Found no legal moves.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _game.GetLegalMoves().Returns(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("list");

        Assert.Equal("fail", result.Message);
        Assert.Equal(UiContent.Moves, result.ContentType);
    }
}
