using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;
using KnightShift.Cli.Tests.Helpers;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class RedoCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly RedoCommand _command;

    public RedoCommandTests()
    {
        _command = new RedoCommand(_game);
    }

    [Theory]
    [InlineData("redo")]
    [InlineData("r")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Redo_Move()
    {
        var state = new GameStateDto
        {
            LastMove = TestData.MoveDto("e2", "e4")
        };

        _game.GetState().Returns(state);

        var result = await _command.ExecuteAsync("redo");

        _game.Received().RedoMove();
        Assert.True(result.RefreshGameState);
        Assert.Equal("Move e2e4 redone.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _game.When(service => service.RedoMove())
            .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("redo");

        Assert.Equal("fail", result.Message);
    }
}
