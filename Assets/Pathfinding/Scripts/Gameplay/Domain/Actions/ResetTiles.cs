using Pathfinding.Scripts.Gameplay.Domain.Repositories;

namespace Pathfinding.Scripts.Gameplay.Domain.Actions
{
    public class ResetTiles
    {
        readonly SelectedTilesRepository selectedTilesRepository;

        public ResetTiles(SelectedTilesRepository selectedTilesRepository) => 
            this.selectedTilesRepository = selectedTilesRepository;

        public void Do() => selectedTilesRepository.Clear();
    }
}