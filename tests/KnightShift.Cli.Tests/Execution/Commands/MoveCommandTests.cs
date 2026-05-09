using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.PlayMove;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Domain.Core;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class MoveCommandTests
{
    private readonly ICommandHandler<PlayMoveCommand> _handler =
        Substitute.For<ICommandHandler<PlayMoveCommand>>();

    private readonly IMoveSerializer _serializer =
        Substitute.For<IMoveSerializer>();

    private readonly MoveCommand _command;

    public MoveCommandTests()
    {
        _command = new MoveCommand(_handler, _serializer);
    }

    [Fact]
    public void CanHandle_Should_Return_True_For_Valid_Move()
    {
        _serializer.TryDeserialize("e2e4", out Move? move).Returns(true);

        var result = _command.CanHandle("e2e4");

        Assert.True(result);
    }

    [Fact]
    public void CanHandle_Should_Return_False_For_Invalid_Move()
    {
        _serializer.TryDeserialize("invalid", out Move? move).Returns(false);

        var result = _command.CanHandle("invalid");

        Assert.False(result);
    }

    [Fact]
    public async Task Execute_Should_Apply_Move()
    {
        var result = await _command.ExecuteAsync("e2e4");

        _handler.Received().Handle(Arg.Any<PlayMoveCommand>());

        Assert.True(result.RefreshGameState);
        Assert.Equal("Move e2e4 was played.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Command_Format()
    {
        await _command.ExecuteAsync("move e2e4");

        _handler.Received().Handle(Arg.Any<PlayMoveCommand>());
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _handler.When(handler => handler.Handle(Arg.Any<PlayMoveCommand>()))
            .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("e2e4");

        Assert.Equal("fail", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Missing_Move()
    {
        var result = await _command.ExecuteAsync("move");

        Assert.Equal("No move provided.", result.Message);
    }
}
