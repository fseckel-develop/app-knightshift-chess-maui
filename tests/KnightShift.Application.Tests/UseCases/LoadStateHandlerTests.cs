using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.LoadState;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class LoadStateHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly LoadStateHandler _handler;

    public LoadStateHandlerTests()
    {
        _handler = new LoadStateHandler(_game);
    }

    [Fact]
    public void Handle_Should_Load_State()
    {
        var fen = "8/8/8/8/8/8/8/8 w - -";

        _handler.Handle(new LoadStateCommand(fen));

        _game.Received().LoadState(fen);
    }
}
