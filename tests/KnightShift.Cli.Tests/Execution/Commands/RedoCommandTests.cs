using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.RedoMove;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Tests.Helpers;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class RedoCommandTests
{
    private readonly IQueryHandler<RedoMoveCommand, MoveDto?> _handler =
        Substitute.For<IQueryHandler<RedoMoveCommand, MoveDto?>>();

    private readonly RedoCommand _command;

    public RedoCommandTests()
    {
        _command = new RedoCommand(_handler);
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
        _handler.Handle(Arg.Any<RedoMoveCommand>()).Returns(TestData.CreateMoveDto("e2", "e4"));

        var result = await _command.ExecuteAsync("redo");

        _handler.Received().Handle(Arg.Any<RedoMoveCommand>());

        Assert.True(result.RefreshGameState);
        Assert.Equal("Move e2e4 redone.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _handler.When(handler => handler.Handle(Arg.Any<RedoMoveCommand>()))
            .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("redo");

        Assert.Equal("fail", result.Message);
    }
}
