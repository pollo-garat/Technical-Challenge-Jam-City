using System.Collections.Generic;

namespace Pathfinding.Scripts.Gameplay.Domain.Repositories
{
    public interface SelectedTilesRepository
    {
        IEnumerable<(int, int)> Load();
        IEnumerable<(int, int)> Save(IEnumerable<(int, int)> selectedTiles);
    }
}