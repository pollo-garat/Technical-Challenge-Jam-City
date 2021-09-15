using NSubstitute;
using NUnit.Framework;
using Pathfinding.Scripts.Gameplay.Domain.Actions;
using Pathfinding.Scripts.Gameplay.Domain.Services;
using static Pathfinding.Scripts.Gameplay.Tests.Mothers.HexaTileMother;
using static Pathfinding.Scripts.Gameplay.Tests.Mothers.RandomServiceMother;

namespace Pathfinding.Scripts.Gameplay.Tests
{
    [TestFixture]
    public class StartGameTests
    {
        [Test]
        public void CreateAGridFullOfTiles()
        {
            var gridService = new GridService(ARandomTileService());
            var startGameAction = new StartGame(gridService);
            var gridCols = 3;
            var gridRows = 3;
            var expectedGrid = new[,]
            {
                {AHexaTile(0, 0), AHexaTile(0,1), AHexaTile(0, 2)},
                {AHexaTile(1, 0), AHexaTile(1,1), AHexaTile(1, 2)},
                {AHexaTile(2, 0), AHexaTile(2,1), AHexaTile(2, 2)},
            };

            var grid = startGameAction.Do(gridRows, gridCols);
            
            Assert.AreEqual(expectedGrid, grid);
        }
        
        [Test]
        public void RandomTileServiceIsCalled()
        {
            var randomService = ARandomTileService();
            var startGameAction = new StartGame(new GridService(randomService));
            var gridCols = 3;
            var gridRows = 3;

            startGameAction.Do(gridRows, gridCols);

            randomService.Received(gridCols * gridCols).PickOne();
        }
    }
}
