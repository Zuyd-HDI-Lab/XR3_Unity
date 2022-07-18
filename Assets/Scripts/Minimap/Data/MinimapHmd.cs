namespace Minimap.Data
{
    public class MinimapHmd: MinimapPositionRotation
    {

    }

    public interface IMinimapItem
    {
        public string Name { get; set; }

        public MinimapItemType MinimapItemType { get; set; }

        public MinimapPosition Position { get; set; }

        public float Rotation { get; set; }
    }

    public enum MinimapItemType
    {
        HMD,
        LeftController,
        RightController,
        Object
    }
}