using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using static Pathfinding.Scripts.Gameplay.Tests.Mothers.HexaTileConfigurationMother;

namespace Pathfinding.Scripts.Gameplay.Tests.Mothers
{
    public static class HexaTileMother
    {
        public static HexaTile AHexaTile(
            int? withXCoordinate = null,
            int? withYCoordinate = null,
            HexaTileConfiguration? withConfiguration = null
        ) =>
            new HexaTile(
                withConfiguration ?? AHexaTileConfiguration(),
                withXCoordinate ?? 0,
                withYCoordinate ?? 0
            );
    }
}