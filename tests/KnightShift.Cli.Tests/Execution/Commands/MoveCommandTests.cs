using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class MoveCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly IMoveSerializer _serializer = Substitute.For<IMoveSerializer>();
    private readonly MoveCommand _command;

    public MoveCommandTests()
    {
        _command = new MoveCommand(_game, _serializer);
    }

    [Fact]
    public void CanHandle_Should_Return_True_For_Valid_Move()
    {
        _serializer.TryDeserialize("e2e4", out _).Returns(true);

        var result = _command.CanHandle("e2e4");

        Assert.True(result);
    }

    [Fact]
    public void CanHandle_Should_Return_False_For_Invalid_Move()
    {
        _serializer.TryDeserialize("invalid", out _).Returns(false);

        var result = _command.CanHandle("invalid");

        Assert.False(result);
    }

    [Fact]
    public async Task Execute_Should_Apply_Move()
    {
        var result = await _command.ExecuteAsync("e2e4");

        _game.Received().ApplyMove("e2e4");
        Assert.True(result.RefreshGameState);
        Assert.Equal("Move e2e4 was played.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Command_Format()
    {
        var result = await _command.ExecuteAsync("move e2e4");

        _game.Received().ApplyMove("e2e4");
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _game.When(service => service.ApplyMove("e2e4"))
             .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("e2e4");

        Assert.Equal("fail", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Throw_When_No_Move_Provided()
    {
        var result = await _command.ExecuteAsync("move");

        Assert.Equal("No move provided.", result.Message);
    }
}
