using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game.Models;
using KnightShift.Application.UseCases.GetHistory;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class GetHistoryHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly GetHistoryHandler _handler;

    public GetHistoryHandlerTests()
    {
        _handler = new GetHistoryHandler(_game);
    }

    [Fact]
    public void Handle_Should_Return_History()
    {
        var history = new List<MoveStep>();

        _game.GetHistory().Returns(history);

        var result = _handler.Handle(new GetHistoryQuery());

        Assert.Equal(history, result);
        _game.Received().GetHistory();
    }
}
