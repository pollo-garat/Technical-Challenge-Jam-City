using NSubstitute;
using NUnit.Framework;
using Pathfinding.Scripts.Gameplay.Domain.Actions;
using Pathfinding.Scripts.Gameplay.Domain.Repositories;

namespace Pathfinding.Scripts.Gameplay.Tests.Actions
{
    [TestFixture]
    public class ResetSelectedTilesTests
    {
        [Test]
        public void ResetSelectedTiles()
        {
            var selectedTilesRepository = Substitute.For<SelectedTilesRepository>();
            var resetTiles = new ResetTiles(selectedTilesRepository);

            resetTiles.Do();

            selectedTilesRepository.Received(1).Clear();
        }
    }
}