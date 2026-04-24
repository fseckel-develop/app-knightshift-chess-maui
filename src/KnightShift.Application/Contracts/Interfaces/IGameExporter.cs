using KnightShift.Application.Game;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameExporter
{
    string Export(GameRecord record);
}
