using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases.GetMoves;
using NSubstitute;

namespace KnightShift.Application.Tests.UseCases;

public class GetMovesHandlerTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly GetMovesHandler _handler;

    public GetMovesHandlerTests()
    {
        _handler = new GetMovesHandler(_game);
    }

    [Fact]
    public void Handle_Should_Return_All_Moves_When_Origin_Is_Null()
    {
        var moves = new List<MoveDto>
        {
            new() { Origin = "e2", Target = "e4" }
        };

        _game.GetLegalMoves().Returns(moves);

        var result = _handler.Handle(new GetMovesQuery()).ToList();

        Assert.Single(result);

        _game.Received().GetLegalMoves();
    }

    [Fact]
    public void Handle_Should_Return_Filtered_Moves_When_Origin_Is_Set()
    {
        var moves = new List<MoveDto>
        {
            new() { Origin = "e2", Target = "e4" }
        };

        _game.GetLegalMoves("e2").Returns(moves);

        var result = _handler.Handle(new GetMovesQuery("e2")).ToList();

        Assert.Single(result);

        _game.Received().GetLegalMoves("e2");
    }
}
