using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.ExportGame;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class ExportGameHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly ExportGameHandler _handler;

    public ExportGameHandlerTests()
    {
        _handler = new ExportGameHandler(_game);
    }

    [Fact]
    public void Handle_Should_Return_Exported_Game()
    {
        _game.ExportGame().Returns("pgn-data");

        var result = _handler.Handle(new ExportGameQuery());

        Assert.Equal("pgn-data", result);
        _game.Received().ExportGame();
    }
}
