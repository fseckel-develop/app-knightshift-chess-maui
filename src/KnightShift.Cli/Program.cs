using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.DependencyInjection;
using KnightShift.Infrastructure.DependencyInjection;
using KnightShift.Engine.DependencyInjection;
using KnightShift.Cli.DependencyInjection;
using KnightShift.Cli.Execution;

namespace KnightShift.Cli;

public static class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services
            .AddApplication()
            .AddInfrastructure()
            .AddEngine()
            .AddCli();

        var provider = services.BuildServiceProvider();

        var app = provider.GetRequiredService<App>();
        app.Run();
    }
}
