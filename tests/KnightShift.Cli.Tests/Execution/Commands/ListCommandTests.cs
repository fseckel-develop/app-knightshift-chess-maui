using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.GetMoves;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Tests.Helpers;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class ListCommandTests
{
    private readonly IQueryHandler<GetMovesQuery, IEnumerable<MoveDto>> _handler =
        Substitute.For<IQueryHandler<GetMovesQuery, IEnumerable<MoveDto>>>();

    private readonly ListCommand _command;

    public ListCommandTests()
    {
        _command = new ListCommand(_handler);
    }

    [Theory]
    [InlineData("list")]
    [InlineData("moves")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_List_All_Moves()
    {
        _handler.Handle(Arg.Any<GetMovesQuery>()).Returns([
            new MoveDto { Origin = "a2", Target = "a3" },
            new MoveDto { Origin = "a2", Target = "a4" }
        ]);

        var result = await _command.ExecuteAsync("list");

        Assert.Equal(UiContent.Moves, result.ContentType);
        Assert.Equal("Found 2 legal moves.", result.Message);

        var state = Assert.IsType<MovesContentState>(result.ContentState);
        Assert.Null(state.OriginSquare);
    }

    [Fact]
    public async Task Execute_Should_List_Moves_From_Origin()
    {
        _handler.Handle(Arg.Any<GetMovesQuery>()).Returns([TestData.CreateMoveDto("e2", "e4")]);

        var result = await _command.ExecuteAsync("list e2");

        Assert.Equal("Found 1 legal move from e2.", result.Message);

        var state = Assert.IsType<MovesContentState>(result.ContentState);
        Assert.Equal("e2", state.OriginSquare);
    }

    [Fact]
    public async Task Execute_Should_Handle_No_Moves()
    {
        _handler.Handle(Arg.Any<GetMovesQuery>()).Returns([]);

        var result = await _command.ExecuteAsync("list");

        Assert.Equal("Found no legal moves.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _handler.When(handler => handler.Handle(Arg.Any<GetMovesQuery>()))
            .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("list");

        Assert.Equal(UiContent.Moves, result.ContentType);
        Assert.Equal("fail", result.Message);
    }
}
