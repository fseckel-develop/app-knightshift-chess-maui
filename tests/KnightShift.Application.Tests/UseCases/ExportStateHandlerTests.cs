using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.ExportState;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class ExportStateHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly ExportStateHandler _handler;

    public ExportStateHandlerTests()
    {
        _handler = new ExportStateHandler(_game);
    }

    [Fact]
    public void Handle_Should_Return_Exported_State()
    {
        _game.ExportState().Returns("fen-data");

        var result = _handler.Handle(new ExportStateQuery());

        Assert.Equal("fen-data", result);
        _game.Received().ExportState();
    }
}