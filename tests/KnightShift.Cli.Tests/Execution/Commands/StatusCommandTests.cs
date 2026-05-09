using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.GetState;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class StatusCommandTests
{
    private readonly IQueryHandler<GetStateQuery, GameStateDto> _handler =
        Substitute.For<IQueryHandler<GetStateQuery, GameStateDto>>();

    private readonly StatusCommand _command;

    public StatusCommandTests()
    {
        _command = new StatusCommand(_handler);
    }

    [Theory]
    [InlineData("status")]
    [InlineData("info")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Show_Turn()
    {
        _handler.Handle(Arg.Any<GetStateQuery>()).Returns(new GameStateDto
        {
            GameResult = GameResultDto.Ongoing,
            CurrentTurn = PieceColorDto.White,
            CurrentIsInCheck = false
        });

        var result = await _command.ExecuteAsync("status");

        Assert.Equal("Turn: White", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Show_Check()
    {
        _handler.Handle(Arg.Any<GetStateQuery>()).Returns(new GameStateDto
        {
            GameResult = GameResultDto.Ongoing,
            CurrentTurn = PieceColorDto.Black,
            CurrentIsInCheck = true
        });

        var result = await _command.ExecuteAsync("status");

        Assert.Equal("Turn: Black (Check!)", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Show_GameOver()
    {
        _handler.Handle(Arg.Any<GetStateQuery>()).Returns(new GameStateDto
        {
            GameResult = GameResultDto.WhiteWins,
            GameEndReason = GameEndReasonDto.Checkmate
        });

        var result = await _command.ExecuteAsync("status");

        Assert.Equal("Game over: Checkmate", result.Message);
    }
}
