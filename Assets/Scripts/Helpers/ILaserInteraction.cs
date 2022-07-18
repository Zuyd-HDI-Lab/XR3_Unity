using Valve.VR;

namespace Helpers
{
    public interface ILaserInteraction
    {
        /// <summary>
        /// Laser pointer click on object
        /// </summary>
        /// <param name="laserEventArgs"></param>
        void OnLaserClick(LaserEventArgs laserEventArgs);

        /// <summary>
        /// Laser pointer in on object
        /// </summary>
        /// <param name="laserEventArgs"></param>
        void OnLaserIn(LaserEventArgs laserEventArgs);

        /// <summary>
        /// Laser pointer out of object
        /// </summary>
        /// <param name="laserEventArgs"></param>
        void OnLaserOut(LaserEventArgs laserEventArgs);
    }

    public class LaserEventArgs
    {
        /// <summary>
        /// Laser pointer hand origin
        /// </summary>
        public SteamVR_Input_Sources SteamVRInputSource { get; set; }
    }
}
