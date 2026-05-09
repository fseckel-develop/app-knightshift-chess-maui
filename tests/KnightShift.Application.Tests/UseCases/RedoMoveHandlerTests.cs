using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.RedoMove;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class RedoMoveHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly RedoMoveHandler _handler;

    public RedoMoveHandlerTests()
    {
        _handler = new RedoMoveHandler(_game);
    }

    [Fact]
    public void Handle_Should_Redo_Move_And_Return_Last_Move()
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

        var result = _handler.Handle(new RedoMoveCommand());

        Assert.Equal(move, result);

        _game.Received().RedoMove();
        _game.Received().GetState();
    }
}
