using System.Linq;
using NUnit.Framework;
using Pathfinding.Scripts.Gameplay.Domain.Actions;
using Pathfinding.Scripts.Gameplay.Domain.Infrastructure;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using static Pathfinding.Scripts.Gameplay.Tests.Mothers.HexaTileConfigurationMother;
using static Pathfinding.Scripts.Gameplay.Tests.Mothers.HexaTileMother;

namespace Pathfinding.Scripts.Gameplay.Tests.Actions
{
    [TestFixture]
    public class SelectTileTests
    {
        [Test]
        public void SelectInitialTile()
        {
            var tileSelected = AHexaTile(0, 0);
            var expectedTileSelected = new[] {AHexaTile(0, 0)};
            var selectedTilesRepository = new InMemorySelectedTilesRepository();
            var selectTiles = new SelectTiles(selectedTilesRepository);

            var selectedTiles = selectTiles.Do(tileSelected);
            
            Assert.AreEqual(expectedTileSelected, selectedTiles);
        }

        [Test]
        public void SelectDestinationTile()
        {
            var initialTile = AHexaTile(0, 0);
            var tileSelected = AHexaTile(2, 3);
            var expectedTileSelected = new[] {initialTile, tileSelected};
            var selectedTilesRepository = new InMemorySelectedTilesRepository(new []{initialTile});
            var selectTiles = new SelectTiles(selectedTilesRepository);

            var selectedTiles = selectTiles.Do(tileSelected);
            
            Assert.AreEqual(expectedTileSelected, selectedTiles);
        }

        [Test]
        public void InitialSelectionCanNotBeWaterTile()
        {
            var tileSelected = AHexaTile(2, 3, AHexaTileConfiguration(withTileType: TileType.Water));
            var expectedTileSelected = Enumerable.Empty<HexaTile>();
            var selectedTilesRepository = new InMemorySelectedTilesRepository();
            var selectTiles = new SelectTiles(selectedTilesRepository);

            var selectedTiles = selectTiles.Do(tileSelected);
            
            Assert.AreEqual(expectedTileSelected, selectedTiles);
        }

        [Test]
        public void DestinationCanNotBeWaterTile()
        {
            var initialTile = AHexaTile(0, 0);
            var tileSelected = AHexaTile(2, 3, AHexaTileConfiguration(withTileType: TileType.Water));
            var expectedTileSelected = new[] {initialTile};
            var selectedTilesRepository = new InMemorySelectedTilesRepository(new []{initialTile});
            var selectTiles = new SelectTiles(selectedTilesRepository);

            var selectedTiles = selectTiles.Do(tileSelected);
            
            Assert.AreEqual(expectedTileSelected, selectedTiles);
        }
    }
}