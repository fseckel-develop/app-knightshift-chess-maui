using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class FenCommandTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly FenCommand _command;

    public FenCommandTests()
    {
        _command = new FenCommand(_game);
    }

    [Fact]
    public void CanHandle_Should_Return_True()
    {
        Assert.True(_command.CanHandle("fen file"));
        Assert.True(_command.CanHandle("save-state file"));
    }

    [Fact]
    public async Task Execute_Should_Return_Error_When_No_File()
    {
        var result = await _command.ExecuteAsync("fen");

        Assert.Equal("No file name provided.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Return_Error_When_Invalid_File()
    {
        var result = await _command.ExecuteAsync("fen invalid name");

        Assert.Equal("Invalid file name.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Save_File_With_Extension()
    {
        var file = Path.GetTempFileName();
        var fileWithoutExtension = Path.ChangeExtension(file, null);

        _game.ExportState().Returns("test-fen");

        var result = await _command.ExecuteAsync($"fen {fileWithoutExtension}");

        var expectedFile = fileWithoutExtension + ".fen";

        Assert.True(File.Exists(expectedFile));
        Assert.Equal("test-fen", File.ReadAllText(expectedFile));
        Assert.Contains(expectedFile, result.Message);

        File.Delete(expectedFile);
    }

    [Fact]
    public async Task Execute_Should_Save_File_With_Existing_Extension()
    {
        var file = Path.GetTempFileName() + ".fen";

        _game.ExportState().Returns("fen-data");

        var result = await _command.ExecuteAsync($"fen {file}");

        Assert.True(File.Exists(file));
        Assert.Equal("fen-data", File.ReadAllText(file));

        File.Delete(file);
    }
}
