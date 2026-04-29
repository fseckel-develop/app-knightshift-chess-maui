using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Tests.Helpers;

namespace KnightShift.Cli.Tests.Rendering.Content;

public class HelpContentProviderTests
{
    [Fact]
    public void GetContent_Should_Order_By_Category_And_Order()
    {
        var commands = new[]
        {
            TestCommandFactory.Create("undo", "Game", 2),
            TestCommandFactory.Create("move", "Game", 0),
            TestCommandFactory.Create("help", "System", 1),
            TestCommandFactory.Create("list", "View", 0)
        };

        var provider = new HelpContentProvider(commands);

        var state = new UiState { Mode = UiMode.Sequential };

        var result = provider.GetContent(state);

        var joined = string.Join("\n", result);

        Assert.Contains("move", joined);
        Assert.Contains("undo", joined);
        Assert.True(joined.IndexOf("move") < joined.IndexOf("undo")); // order
    }

    [Fact]
    public void GetContent_Should_Include_Uci_Shortcut()
    {
        var provider = new HelpContentProvider([]);

        var state = new UiState { Mode = UiMode.Sequential };

        var result = provider.GetContent(state);

        Assert.Contains("<uci>", string.Join("\n", result));
    }

    [Fact]
    public void GetContent_Should_Add_Empty_Lines_In_Dashboard_Mode()
    {
        var commands = new[] { TestCommandFactory.Create("move", "Game", 0) };
        var provider = new HelpContentProvider(commands);

        var state = new UiState { Mode = UiMode.Dashboard };

        var result = provider.GetContent(state);

        Assert.Equal("", result[0]);
    }

    [Fact]
    public void BuildCommandLabel_Should_Format_Correctly()
    {
        var command = TestCommandFactory.Create("move", "Game", 0, parameter: "{uci}", aliases: ["m"]);

        var provider = new HelpContentProvider([command]);

        var result = provider.GetContent(new UiState());

        var line = result.First(line => line.Contains("move (m)"));

        Assert.Contains("move (m) {uci}", line);
    }
}
