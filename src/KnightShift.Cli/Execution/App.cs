namespace KnightShift.Cli.Execution;

public class App
{
    private readonly CommandLoop _loop;

    public App(CommandLoop loop)
    {
        _loop = loop;
    }

    public void Run()
    {
        Console.Title = "KnightShift CLI";
        _loop.RunAsync().GetAwaiter().GetResult();
    }
}
