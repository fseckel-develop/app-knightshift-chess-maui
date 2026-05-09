using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.GetState;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class GetStateHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly GetStateHandler _handler;

    public GetStateHandlerTests()
    {
        _handler = new GetStateHandler(_game);
    }

    [Fact]
    public void Handle_Should_Return_Game_State()
    {
        var state = new GameStateDto();

        _game.GetState().Returns(state);

        var result = _handler.Handle(new GetStateQuery());

        Assert.Equal(state, result);

        _game.Received().GetState();
    }
}
