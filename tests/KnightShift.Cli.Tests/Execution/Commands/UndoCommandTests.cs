using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.UndoMove;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Tests.Helpers;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class UndoCommandTests
{
    private readonly IQueryHandler<UndoMoveCommand, MoveDto?> _handler =
        Substitute.For<IQueryHandler<UndoMoveCommand, MoveDto?>>();

    private readonly UndoCommand _command;

    public UndoCommandTests()
    {
        _command = new UndoCommand(_handler);
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
        _handler.Handle(Arg.Any<UndoMoveCommand>()).Returns(TestData.CreateMoveDto("e2", "e4"));

        var result = await _command.ExecuteAsync("undo");

        _handler.Received().Handle(Arg.Any<UndoMoveCommand>());

        Assert.True(result.RefreshGameState);
        Assert.Equal("Move e2e4 undone.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _handler.When(handler => handler.Handle(Arg.Any<UndoMoveCommand>()))
            .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("undo");

        Assert.Equal("fail", result.Message);
    }
}
