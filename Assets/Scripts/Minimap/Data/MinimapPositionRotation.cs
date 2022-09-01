namespace Minimap.Data
{
    public class MinimapPositionRotation : IMinimapItem
    {
        public string Name { get; set; }
        public MinimapItemType MinimapItemType { get; set; }
        public MinimapPosition Position { get; set; } = new();
        public float Rotation { get; set; }
    }
}
