using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class HelpCommandTests
{
    private readonly HelpCommand _command = new();

    [Theory]
    [InlineData("help")]
    [InlineData("h")]
    [InlineData("?")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Return_Message_When_No_Commands()
    {
        var result = await _command.ExecuteAsync("help");

        Assert.Equal("No commands available.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Return_Help_Content()
    {
        var commands = new List<ICommand>
        {
            new ExitCommand()
        };

        _command.SetCommands(commands);

        var result = await _command.ExecuteAsync("help");

        Assert.Equal(UiContent.Help, result.ContentType);
        Assert.Equal("Showing available commands.", result.Message);
    }
}
