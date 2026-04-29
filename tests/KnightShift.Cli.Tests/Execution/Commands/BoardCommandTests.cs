using KnightShift.Cli.Execution.Commands;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class BoardCommandTests
{
    private readonly BoardCommand _command = new();

    [Theory]
    [InlineData("board")]
    [InlineData("BOARD")]
    [InlineData("display")]
    public void CanHandle_Should_Return_True_For_Valid_Commands(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public void CanHandle_Should_Return_False_For_Invalid_Command()
    {
        Assert.False(_command.CanHandle("b"));
    }

    [Fact]
    public async Task Execute_Should_Enable_AutoPrint()
    {
        var result = await _command.ExecuteAsync("board on");

        Assert.True(result.AutoPrintBoard);
        Assert.Equal("Auto-printing of board enabled.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Disable_AutoPrint()
    {
        var result = await _command.ExecuteAsync("board off");

        Assert.False(result.AutoPrintBoard);
        Assert.Equal("Auto-printing of board disabled.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_Return_Error_For_Invalid_State()
    {
        var result = await _command.ExecuteAsync("board maybe");

        Assert.Null(result.AutoPrintBoard);
        Assert.Equal("Unkown auto-printing state.", result.Message);
    }

    [Fact]
    public async Task Execute_Should_PrintBoard_When_No_Argument()
    {
        var result = await _command.ExecuteAsync("board");

        Assert.True(result.PrintBoard);
        Assert.Equal("Showing current board state.", result.Message);
    }
}
