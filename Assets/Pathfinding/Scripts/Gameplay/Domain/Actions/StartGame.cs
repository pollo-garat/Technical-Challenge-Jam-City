using Pathfinding.Scripts.Gameplay.Domain.Services;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Domain.Actions
{
    public class StartGame
    {
        readonly GridService gridService;

        public StartGame(GridService gridService)
        {
            this.gridService = gridService;
        }

        public HexaTile[,] Do(int gridRows, int gridCols) => 
            gridService.Create(gridRows, gridCols);
    }
}