using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.NewGame;
using KnightShift.Cli.Execution.Commands;
using NSubstitute;

namespace KnightShift.Cli.Tests.Execution.Commands;

public class NewCommandTests
{
    private readonly ICommandHandler<NewGameCommand> _handler =
        Substitute.For<ICommandHandler<NewGameCommand>>();

    private readonly NewCommand _command;

    public NewCommandTests()
    {
        _command = new NewCommand(_handler);
    }

    [Theory]
    [InlineData("new")]
    [InlineData("n")]
    [InlineData("reset")]
    [InlineData("start")]
    public void CanHandle_Should_Return_True(string input)
    {
        Assert.True(_command.CanHandle(input));
    }

    [Fact]
    public async Task Execute_Should_Start_New_Game()
    {
        var result = await _command.ExecuteAsync("new");

        _handler.Received().Handle(Arg.Any<NewGameCommand>());

        Assert.True(result.RefreshGameState);
        Assert.Equal("New game started.", result.Message);
    }
}
