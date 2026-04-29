using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class NewCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly NewCommand _command;

    public NewCommandTests()
    {
        _command = new NewCommand(_game);
    }

    [Theory]
    [InlineData("new")]
    [InlineData("n")]
    [InlineData("reset")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Start_New_Game()
    {
        var result = await _command.ExecuteAsync("new");

        _game.Received().StartNewGame();
        Assert.True(result.RefreshGameState);
        Assert.Equal("New game started.", result.Message);
    }
}
