using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.Repositories;

namespace Pathfinding.Scripts.Gameplay.Domain.Infrastructure
{
    public class InMemorySelectedTilesRepository : SelectedTilesRepository
    {
        IEnumerable<(int, int)> selectedTiles;

        public InMemorySelectedTilesRepository(IEnumerable<(int, int)> selectedTiles = null) => 
            this.selectedTiles = selectedTiles ?? Enumerable.Empty<(int, int)>();

        public IEnumerable<(int, int)> Load() => 
            selectedTiles;

        public IEnumerable<(int, int)> Save(IEnumerable<(int, int)> selectedTiles) =>
            this.selectedTiles = selectedTiles;
    }
}