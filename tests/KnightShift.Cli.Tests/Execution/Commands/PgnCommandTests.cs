using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class PgnCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly PgnCommand _command;

    public PgnCommandTests()
    {
        _command = new PgnCommand(_game);
    }

    [Fact]
    public async Task Execute_Should_Return_Error_When_No_File()
    {
        var result = await _command.ExecuteAsync("pgn");

        Assert.Equal("No file name provided.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Return_Error_When_Invalid_File()
    {
        var result = await _command.ExecuteAsync("pgn invalid name");

        Assert.Equal("Invalid file name.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Save_File_With_Extension()
    {
        var file = Path.GetTempFileName();
        var fileWithoutExtension = Path.ChangeExtension(file, null);

        _game.ExportGame().Returns("test-pgn");

        var result = await _command.ExecuteAsync($"pgn {fileWithoutExtension}");

        var expectedFile = fileWithoutExtension + ".pgn";

        Assert.True(File.Exists(expectedFile));
        Assert.Equal("test-pgn", File.ReadAllText(expectedFile));
        Assert.Contains(expectedFile, result.Message);

        File.Delete(expectedFile);
    }

    [Fact]
    public async Task Execute_Should_Save_File_With_Existing_Extension()
    {
        var file = Path.GetTempFileName() + ".pgn";

        _game.ExportGame().Returns("pgn-data");

        var result = await _command.ExecuteAsync($"pgn {file}");

        Assert.True(File.Exists(file));
        Assert.Equal("pgn-data", File.ReadAllText(file));

        File.Delete(file);
    }
}
