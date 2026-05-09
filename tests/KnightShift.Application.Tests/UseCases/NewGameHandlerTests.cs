using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.NewGame;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class NewGameHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly NewGameHandler _handler;

    public NewGameHandlerTests()
    {
        _handler = new NewGameHandler(_game);
    }

    [Fact]
    public void Handle_Should_Start_New_Game()
    {
        _handler.Handle(new NewGameCommand());

        _game.Received().StartNewGame();
    }
}
