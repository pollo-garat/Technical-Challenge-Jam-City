namespace Pathfinding.Scripts.Gameplay.Domain.ValueObjects
{
    public struct HexaTileConfiguration
    {
        public TileType Type;
        public int Cost;
        public bool IsWalkable;

        public HexaTileConfiguration(TileType type, int cost, bool isWalkable)
        {
            Type = type;
            Cost = cost;
            IsWalkable = isWalkable;
        }

        public override string ToString() => 
            $"Type: {Type.ToString()} Cost: {Cost} IsWalkable {IsWalkable}";
    }

    public enum TileType
    {
        Grass,
        Forest,
        Desert,
        Mountain,
        Water
    }
}