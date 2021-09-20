using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.Actions;
using Pathfinding.Scripts.Gameplay.Domain.Infrastructure;
using Pathfinding.Scripts.Gameplay.Domain.Services;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using Pathfinding.Scripts.Gameplay.Domain.Views;
using UniRx;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain
{
    public class Context : MonoBehaviour
    {
        public WorldGrid WorldGrid;
        public AStartPathFinder PathFinder;
        public int GrassCost;
        public int ForestCost;
        public int DesertCost;
        public int MountainCost;
        public bool DebugNeighbours;
        
        ResetTiles resetTiles;

        void Start()
        {
            var GrassConfiguration = new HexaTileConfiguration(TileType.Grass, GrassCost, true);
            var ForestConfiguration = new HexaTileConfiguration(TileType.Forest, ForestCost, true);
            var DesertConfiguration = new HexaTileConfiguration(TileType.Desert, DesertCost, true);
            var MountainConfiguration = new HexaTileConfiguration(TileType.Mountain, MountainCost, true);
            var WaterConfiguration = new HexaTileConfiguration(TileType.Water, 0, false);
            
            var randomTileService = new UnityRandomTileService(
                GrassConfiguration,
                ForestConfiguration,
                DesertConfiguration,
                MountainConfiguration,
                WaterConfiguration
            );
            
            var inMemorySelectedTilesRepository = new InMemorySelectedTilesRepository();
            
            var gridService = new GridService(randomTileService);
            var girdNeighbours = new GridNeighbours(WorldGrid.GridWidth, WorldGrid.GridHeight);
            
            var startGame = new StartGame(gridService);
            var selectTiles = new SelectTiles(inMemorySelectedTilesRepository);
            resetTiles = new ResetTiles(inMemorySelectedTilesRepository);
            
            var domainGrid = startGame.Do(WorldGrid.GridWidth, WorldGrid.GridHeight);

            WorldGrid.CreateGrid(domainGrid, girdNeighbours);
            PathFinder.Initialize(WorldGrid.GetUnityGrid(), girdNeighbours);

            var onTileClicked = WorldGrid.OnTileClicked.Share();
            
            onTileClicked
                .Where(_ => DebugNeighbours)
                .Select(girdNeighbours.ValidNeighbours)
                .Do(PathFinder.PaintNeighbours)
                .Subscribe();
            
            onTileClicked
                .Select(selectTiles.Do)
                .Do(PathFinder.HighlightTile)
                .SelectMany(selectedTiles => 
                    PathAlreadyFound(selectedTiles) ? ResetMap() :
                    PathNotFound(selectedTiles) ? FindPath(selectedTiles) :
                    _
                )
                .Subscribe();
        }

        static bool PathNotFound(IEnumerable<HexaTile> selectedTiles) => 
            selectedTiles.Count() > 1;

        static bool PathAlreadyFound(IEnumerable<HexaTile> selectedTiles) => 
            selectedTiles.Count() > 2;

        IEnumerable<HexaTile> ResetMap()
        {
            PathFinder.ResetMapGraphics();
            resetTiles.Do();
            return _;
        }

        IEnumerable<HexaTile> FindPath(IEnumerable<HexaTile> selectedTiles)
        {
            PathFinder.FindPath(selectedTiles);
            return _;
        }

        static IEnumerable<HexaTile> _ => Enumerable.Empty<HexaTile>();
    }
}
