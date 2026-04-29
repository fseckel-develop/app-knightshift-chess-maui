using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class UndoCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly UndoCommand _command;

    public UndoCommandTests()
    {
        _command = new UndoCommand(_game);
    }

    [Theory]
    [InlineData("undo")]
    [InlineData("u")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Undo_Move()
    {
        var state = new GameStateDto
        {
            LastMove = new MoveDto { Origin = "e2", Target = "e4" }
        };

        _game.GetState().Returns(state);

        var result = await _command.ExecuteAsync("undo");

        _game.Received().UndoMove();
        Assert.True(result.RefreshGameState);
        Assert.Equal("Move e2e4 undone.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _game.When(service => service.UndoMove())
            .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("undo");

        Assert.Equal("fail", result.Message);
    }
}
