using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class UiModeCommandTests
{
    private readonly UiModeCommand _command = new();

    [Theory]
    [InlineData("ui")]
    [InlineData("mode")]
    [InlineData("ui dash")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Return_Error_When_No_Mode()
    {
        var result = await _command.ExecuteAsync("ui");

        Assert.Equal("No UI mode provided.", result.Message);
    }

    [Theory]
    [InlineData("ui dashboard")]
    [InlineData("ui dash")]
    [InlineData("ui d")]
    public async Task Execute_Should_Set_Dashboard_Mode(string input)
    {
        var result = await _command.ExecuteAsync(input);

        Assert.Equal(UiMode.Dashboard, result.Mode);
        Assert.Equal("Switched to dashboard mode.", result.Message);
    }

    [Theory]
    [InlineData("ui sequential")]
    [InlineData("ui seq")]
    [InlineData("ui s")]
    public async Task Execute_Should_Set_Sequential_Mode(string input)
    {
        var result = await _command.ExecuteAsync(input);

        Assert.Equal(UiMode.Sequential, result.Mode);
        Assert.Equal("Switched to sequential mode.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Handle_Invalid_Mode()
    {
        var result = await _command.ExecuteAsync("ui unknown");

        Assert.Equal("Unknown UI mode.", result.Message);
    }
}
