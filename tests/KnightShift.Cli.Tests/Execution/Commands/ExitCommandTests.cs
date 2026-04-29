using KnightShift.Cli.Execution.Commands;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class ExitCommandTests
{
    private readonly ExitCommand _command = new();

    [Theory]
    [InlineData("exit")]
    [InlineData("quit")]
    [InlineData("x")]
    [InlineData("q")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public void CanHandle_Should_Return_False_For_Invalid()
    {
        Assert.False(_command.CanHandle("close"));
    }

    [Fact]
    public async Task Execute_Should_Request_Exit()
    {
        var result = await _command.ExecuteAsync("exit");

        Assert.True(result.ExitRequested);
    }
}
