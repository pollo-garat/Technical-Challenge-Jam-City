using NUnit.Framework;
using Pathfinding.Scripts.Gameplay.Domain.Actions;
using Pathfinding.Scripts.Gameplay.Domain.Infrastructure;

namespace Pathfinding.Scripts.Gameplay.Tests.Actions
{
    [TestFixture]
    public class SelectTileTests
    {
        [Test]
        public void SelectInitialTile()
        {
            var tileSelected = (0, 0);
            var expectedTileSelected = new[] {(0, 0)};
            var selectedTilesRepository = new InMemorySelectedTilesRepository();
            var selectTiles = new SelectTiles(selectedTilesRepository);

            var selectedTiles = selectTiles.Do(tileSelected);
            
            Assert.AreEqual(expectedTileSelected, selectedTiles);
        }

        [Test]
        public void SelectDestinationTile()
        {
            var initialTile = (0, 0);
            var tileSelected = (2, 3);
            var expectedTileSelected = new[] {initialTile, tileSelected};
            var selectedTilesRepository = new InMemorySelectedTilesRepository(new []{initialTile});
            var selectTiles = new SelectTiles(selectedTilesRepository);

            var selectedTiles = selectTiles.Do(tileSelected);
            
            Assert.AreEqual(expectedTileSelected, selectedTiles);
        }
    }
}