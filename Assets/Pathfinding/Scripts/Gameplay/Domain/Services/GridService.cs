using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using static Pathfinding.Scripts.Gameplay.Domain.ValueObjects.HexaTile;

namespace Pathfinding.Scripts.Gameplay.Domain.Services
{
    public class GridService
    {
        readonly RandomTileService randomTileService;

        public GridService(RandomTileService randomTileService)
        {
            this.randomTileService = randomTileService;
        }
        
        public HexaTile[,] Create(int gridRows, int gridCols)
        {
            var grid = new HexaTile[gridRows, gridCols];
            
            for (var y = 0; y < gridRows; y++)
            {
                for (var x = 0; x < gridCols; x++)
                {
                    var hexaTile = randomTileService.PickOne();
                    grid[x, y] = SetTileInCoordinates(hexaTile, x, y);
                }
            }

            return grid;
        }
    }
}