namespace Minimap.Data
{
    public class MinimapController: MinimapPositionRotation
    {
        public MinimapControllerSide Side { get; set; }   
    }

    public enum MinimapControllerSide
    {
        Left,
        Right
    }
}