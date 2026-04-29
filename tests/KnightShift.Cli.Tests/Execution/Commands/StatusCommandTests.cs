using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class StatusCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly StatusCommand _command;

    public StatusCommandTests()
    {
        _command = new StatusCommand(_game);
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
        _game.GetState().Returns(new GameStateDto
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
        _game.GetState().Returns(new GameStateDto
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
        _game.GetState().Returns(new GameStateDto
        {
            GameResult = GameResultDto.WhiteWins,
            GameEndReason = GameEndReasonDto.Checkmate
        });

        var result = await _command.ExecuteAsync("status");

        Assert.Equal("Game over: Checkmate", result.Message);
    }
}
