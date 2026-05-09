using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.LoadGame;
using KnightShift.Application.UseCases.LoadState;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class LoadCommandTests
{
    private readonly ICommandHandler<LoadGameCommand> _loadGameHandler =
        Substitute.For<ICommandHandler<LoadGameCommand>>();

    private readonly ICommandHandler<LoadStateCommand> _loadStateHandler =
        Substitute.For<ICommandHandler<LoadStateCommand>>();

    private readonly LoadCommand _command;

    public LoadCommandTests()
    {
        _command = new LoadCommand(_loadGameHandler, _loadStateHandler);
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
        var fen = "8/8/8/8/8/8/8/8 w - -";

        var result = await _command.ExecuteAsync($"load {fen}");

        _loadStateHandler.Received().Handle(Arg.Any<LoadStateCommand>());

        Assert.True(result.RefreshGameState);
        Assert.Equal("FEN loaded.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Load_PGN()
    {
        var pgn = "1. e4 e5";

        var result = await _command.ExecuteAsync($"load {pgn}");

        _loadGameHandler.Received().Handle(Arg.Any<LoadGameCommand>());

        Assert.True(result.RefreshGameState);
        Assert.Equal("PGN loaded.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Return_Unknown_Format()
    {
        var result = await _command.ExecuteAsync("load unknown");

        Assert.Equal("Unknown format.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Exception()
    {
        _loadStateHandler.When(handler => handler.Handle(Arg.Any<LoadStateCommand>()))
            .Do(_ => throw new Exception("fail"));

        var fen = "8/8/8/8/8/8/8/8 w - -";

        var result = await _command.ExecuteAsync($"load {fen}");

        Assert.Equal("fail", result.Message);
    }
}
