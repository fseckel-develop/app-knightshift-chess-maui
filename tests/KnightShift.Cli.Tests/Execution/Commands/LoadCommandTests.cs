using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class LoadCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly LoadCommand _command;

    public LoadCommandTests()
    {
        _command = new LoadCommand(_game);
    }

    [Fact]
    public async Task Execute_Should_Return_Error_When_No_Input()
    {
        var result = await _command.ExecuteAsync("load");

        Assert.Equal("No payload or file name provided.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Load_FEN()
    {
        var fen = "8/8/8/8/8/8/8/8 w - - 0 1";

        var result = await _command.ExecuteAsync($"load {fen}");

        _game.Received().LoadState(fen);
        Assert.True(result.RefreshGameState);
        Assert.Equal("FEN loaded.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Load_PGN()
    {
        var pgn = "1. e4 e5";

        var result = await _command.ExecuteAsync($"load {pgn}");

        _game.Received().LoadGame(pgn);
        Assert.Equal("PGN loaded.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Return_Unknown_Format()
    {
        var result = await _command.ExecuteAsync("load something");

        Assert.Equal("Unknown format.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _game.When(service => service.LoadState(Arg.Any<string>()))
             .Do(_ => throw new Exception("fail"));

        var result = await _command.ExecuteAsync("load 8/8/8/8/8/8/8/8 w - - 0 1");

        Assert.Equal("fail", result.Message);
    }
}
