using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Tests.Mothers
{
    public static class HexaTileConfigurationMother
    {
        public static HexaTileConfiguration AHexaTileConfiguration(
            TileType? withTileType = null,
            int? withCost = null,
            bool isWalkable = true
        ) =>
            new HexaTileConfiguration(
                withTileType ?? TileType.Grass, 
                withCost ?? 1, 
                isWalkable
            );
    }
}