using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.UndoMove;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class UndoMoveHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly UndoMoveHandler _handler;

    public UndoMoveHandlerTests()
    {
        _handler = new UndoMoveHandler(_game);
    }

    [Fact]
    public void Handle_Should_Undo_Move_And_Return_Last_Move()
    {
        var move = new MoveDto
        {
            Origin = "e2",
            Target = "e4"
        };

        _game.GetState().Returns(new GameStateDto
        {
            LastMove = move
        });

        var result = _handler.Handle(new UndoMoveCommand());

        Assert.Equal(move, result);

        _game.Received().UndoMove();
        _game.Received().GetState();
    }
}
