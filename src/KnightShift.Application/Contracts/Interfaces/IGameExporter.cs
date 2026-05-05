using KnightShift.Application.Game.Models;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameExporter
{
    string Export(GameRecord record);
}
