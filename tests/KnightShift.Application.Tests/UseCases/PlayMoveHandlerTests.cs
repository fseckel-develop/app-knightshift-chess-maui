using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.PlayMove;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class PlayMoveHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly PlayMoveHandler _handler;

    public PlayMoveHandlerTests()
    {
        _handler = new PlayMoveHandler(_game);
    }

    [Fact]
    public void Handle_Should_Apply_Move()
    {
        _handler.Handle(new PlayMoveCommand("e2e4"));

        _game.Received().ApplyMove("e2e4");
    }
}
