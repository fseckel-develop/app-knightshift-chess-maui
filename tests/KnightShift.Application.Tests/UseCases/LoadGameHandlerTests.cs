using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.LoadGame;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class LoadGameHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly LoadGameHandler _handler;

    public LoadGameHandlerTests()
    {
        _handler = new LoadGameHandler(_game);
    }

    [Fact]
    public void Handle_Should_Load_Game()
    {
        var pgn = "1. e4 e5";

        _handler.Handle(new LoadGameCommand(pgn));

        _game.Received().LoadGame(pgn);
    }
}
