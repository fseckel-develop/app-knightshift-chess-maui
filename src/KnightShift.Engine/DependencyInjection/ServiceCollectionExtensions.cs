using Microsoft.Extensions.DependencyInjection;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Evaluation;
using KnightShift.Engine.Rules;

namespace KnightShift.Engine.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEngine(this IServiceCollection services)
    {
        services.AddScoped<ICheckDetector, CheckDetector>();
        services.AddScoped<IMoveValidator, MoveValidator>();
        services.AddScoped<IMoveGenerator, MoveGenerator>();
        services.AddScoped<IGameResultEvaluator, GameResultEvaluator>();

        return services;
    }
}
