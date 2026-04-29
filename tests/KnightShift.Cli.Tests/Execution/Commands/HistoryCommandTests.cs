using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Tests.Helpers;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class HistoryCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly HistoryCommand _command;

    public HistoryCommandTests()
    {
        _command = new HistoryCommand(_game);
    }

    [Theory]
    [InlineData("history")]
    [InlineData("san")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Return_Message_When_No_Moves()
    {
        _game.GetHistory().Returns([]);

        var result = await _command.ExecuteAsync("history");

        Assert.Equal("No moves have been played yet.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Singular()
    {
        _game.GetHistory().Returns(TestData.History(1));

        var result = await _command.ExecuteAsync("history");

        Assert.Equal(UiContent.History, result.ContentType);
        Assert.Equal("Tracked 1 move in this game.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Plural()
    {
        _game.GetHistory().Returns(TestData.History(2));

        var result = await _command.ExecuteAsync("history");

        Assert.Equal("Tracked 2 moves in this game.", result.Message);
    }
}
