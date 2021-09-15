using NSubstitute;
using Pathfinding.Scripts.Gameplay.Domain.Services;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using static Pathfinding.Scripts.Gameplay.Tests.Mothers.HexaTileMother;

namespace Pathfinding.Scripts.Gameplay.Tests.Mothers
{
    public static class RandomServiceMother
    {
        public static RandomTileService ARandomTileService(HexaTile? withHexaTile = null)
        {
            var randomTileService = Substitute.For<RandomTileService>();
            randomTileService.PickOne().Returns(withHexaTile ?? AHexaTile());
            return randomTileService;
        }
    }
}